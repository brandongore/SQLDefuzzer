# SQLDefuzzer
SqlDefuzzer is an SSMS extension which can either interface with a locally installed version of the sqlfluff python package 
or interface with an isolated containerised version for improved security

# Building the sqlfluff python image using podman
follow the general guide for installing and getting started with podman.

start the default podman machine
```
podman machine start
```
this assumes the default machine is being used, ssh into machine to run build within machine
```
podman machine ssh
```
The following is an example for navigating to the folder with image config files

Change the path based on your local path
```
cd /mnt/c/dev/SQLDefuzzer/sqlfluffpod
```
Build the image
```
podman build -t sqlfluff-defuzzer-app .
```
create a user-defined bridge network, This isolates the container to its own network, allowing only local communication and preventing outbound connections by default
```
podman network create --driver bridge defuzzer-bridge-network
```
creating a user-defined bridge network for your container

Run the image with the isolated bridge network
```
podman run --network defuzzer-bridge-network -p 5000:5000 sqlfluff-defuzzer-app
```

# Debugging image issues
if there are issues with sqlfluff inside the image, you can run the following commands in another shell to determine if sqlfluff is installed.
```
podman container list
```
copy the container ID from the list for sqlfluff-defuzzer-app container and replace CONTAINERID in following command with copied value.
```
podman exec -it CONTAINERID /bin/sh
```
```
sqlfluff --version
```
you should also be able to execute sqlfluff commands in the same context to check output
```
echo "select a,c from b" | python -m sqlfluff fix - --dialect=tsql
```

## Licence
MIT