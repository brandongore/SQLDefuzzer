from flask import Flask, request, jsonify
import subprocess
import tempfile
import os
import configparser
import shutil
import sys

app = Flask(__name__)

@app.route('/execute', methods=['POST'])
def execute_code():
    try:
        data = request.get_json()

        # Get the SQL code and configuration from the request payload
        code = data['code']
        configuration = data.get('configuration', {})

        # Create a temporary directory
        temp_dir = tempfile.mkdtemp()

        # Create a temporary configuration file with the allowed name
        config_file_path = os.path.join(temp_dir, '.sqlfluff')
        write_config_to_file(config_file_path, configuration)
        
        with open(config_file_path, 'r') as configfile:
            config_content = configfile.read()
            print(f'Configuration file content:\n{config_content}', file=sys.stderr)

        print(f'Config file path: {config_file_path}', file=sys.stderr)
        # Modify the subprocess command to run sqlfluff with the provided SQL code
        result = subprocess.check_output(['sqlfluff', 'fix', '-', f'--config={config_file_path}'],input=code, stderr=subprocess.STDOUT, text=True)
        print(f'sqlfluff result:\n{result}', file=sys.stderr)

        return jsonify({'result': result}), 200
    except Exception as e:
        print(f'Error: {str(e)}')
        return jsonify({'error': str(e)}), 500
    finally:
        # Cleanup: Remove the temporary directory
        if temp_dir and os.path.exists(temp_dir):
            shutil.rmtree(temp_dir)

def write_config_to_file(file_path, configuration):
    # Write the configuration to a temporary file
    config = configparser.ConfigParser()
    config.read_dict(configuration)
    with open(file_path, 'w') as configfile:
        config.write(configfile)

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)