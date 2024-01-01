#!/bin/bash

# Usage:
# - export the pod_name
# - export the password
# - run the script ./backup.sh

# MongoDB connection details
port="27017"
username="root"

# Generate a timestamp for the backup directory
timestamp=$(date +'%Y%m%d%H%M%S')
backupDir="/backup/$timestamp"

# Check if the MongoDB pod is running
if kubectl get pods $pod_name &> /dev/null; then
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