#!/bin/bash

# Usage:
# - cd to Scripts/
# - export pod_name=<pod-name>
# - export password=<mongo-password>
# - run the script ./backup.sh

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
fi

echo "pod_name: $pod_name"
echo "password: $password"

# Generate a timestamp for the backup directory
timestamp=$(date +'%Y%m%d%H%M%S')
backupDir="/backup/$timestamp"

# Check if the MongoDB pod is running
if kubectl get pods $pod_name &> /dev/null; then
    # create the backup dir if not exist
    kubectl exec $pod_name -- mkdir -p /backup
    
    # Create backup
    kubectl exec $pod_name -- mongodump --port $port --username $username --password $password --out $backupDir --verbose

    # Copy the backup to local
    kubectl cp "${pod_name}:${backupDir}" "../Backups/${timestamp}"

    # Compress backup
    # TO DECOMPRESS USE tar -xvzf <FILE-NAME>.tar.gz
    backup_folder_name="../Backups"
    file_name="${timestamp}.tar.gz"
    cd $backup_folder_name
    tar -zcvf ${file_name} ${timestamp}

    # Remove the original folder
    rm -rf ${timestamp}

    echo "Backup completed successfully!"
else
    echo "Error: MongoDB pod '$pod_name' is not running."
fi