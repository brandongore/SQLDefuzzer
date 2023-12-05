from flask import Flask, request, jsonify
from sqlfluff.core import FluffConfig, Linter, SQLBaseError
from typing import List

import sqlfluff

app = Flask(__name__)

def serialize_violations(violations: List[SQLBaseError]) -> List[dict]:
    #Serialize a list of SQLBaseError objects to a list of dictionaries.
    serialized_violations = [violation.get_info_dict() for violation in violations]
    return serialized_violations

@app.route('/fix', methods=['POST'])
def fix_code():
    try:
        data = request.get_json()

        # Get the SQL code and configuration from the request payload
        code = data['code']
        configuration = data.get('configuration', {})
        # Need to add --library-path none to config

        # Create a FluffConfig object from the provided configuration
        config = FluffConfig(configs=configuration)

        # Create a Linter with the provided configuration
        linter = Linter(config=config)

        # Lint and fix the SQL code
        lint_result = linter.lint_string(code, fix=True)
        fixed_code = lint_result.fix_string()

        return jsonify({'result': fixed_code[0], 'success': fixed_code[1]}), 200
    except Exception as e:
        return jsonify({'result': str(e), 'success': False}), 500
    
@app.route('/lint', methods=['POST'])
def lint_code():
    try:
        data = request.get_json()

        # Get the SQL code and configuration from the request payload
        code = data['code']
        configuration = data.get('configuration', {})

        # Create a FluffConfig object from the provided configuration
        config = FluffConfig(configs=configuration)

        # Create a Linter with the provided configuration
        linter = Linter(config=config)

        # Lint the SQL code
        lint_result = linter.lint_string(code, fix=True)
        violations = lint_result.get_violations()

        # Serialize the violations to JSON
        serialized_violations = serialize_violations(violations)

        return jsonify({'result': serialized_violations, 'success': True}), 200
    except Exception as e:
        return jsonify({'result': str(e), 'success': False}), 500

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)