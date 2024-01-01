#!/bin/bash

# Usage:
# - cd to Scripts/
# - export pod_name=<pod-name>
# - export password=<mongo-password>
# - export backup_file_name=<back-file-name>(without .tar.gz)
# - delete the database
# - run the script ./restore.sh

label="app=mongo-service"

# Get the pod name using the correct label
pod_name=$(kubectl get pods -l "$label" -o jsonpath='{.items[0].metadata.name}')

# MongoDB connection details
port="27017"
username="root"

# Check if required environment variables are set
if [[ -z "$pod_name" ]]; then
    echo "Error: Please set the environment variables pod_name."
    exit 1
elif [[ -z "$password" ]]; then
    echo "Error: Please set the environment variables password."
    exit 1
elif [[ -z "$backup_file_name" ]]; then
    echo "Error: Please set the environment variables backup_file_name."
    exit 1
fi

echo "pod_name: $pod_name"
echo "password: $password"
echo "backup_file_name: $backup_file_name"

# Check if required environment variables are set
if [[ -z "$pod_name" || -z "$password" || -z "$backup_file_name" ]]; then
    echo "Error: Please set the environment variables pod_name, password, and backup_file_name."
    exit 1
fi

# Generate a timestamp for the backup directory
local_path="../Backups/${backup_file_name}"
local_compressed_path="${local_path}.tar.gz"
remote_path="/backup/$backup_file_name"

# decompress the backup
tar -xvzf $local_path


# Check if the MongoDB pod is running
if kubectl get pods $pod_name &> /dev/null; then
    # copy the backup folder to remote
    kubectl cp "$backup_file_name:$remote_path"

    # restore the database
    kubectl exec $pod_name -- mongorestore --dir $remote_path --username $username --password $password

    # Remove the original folder
    rm -rf ${backup_file_name}

    echo "Restore completed successfully!"
else
    echo "Error: MongoDB pod '$pod_name' is not running."
fi