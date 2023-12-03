#!/bin/bash

# Check if the file exists in the volume
if [ -f "/lab01/index.html" ]; then
    # Copy the file from the volume to the desired location inside the container
    cp /lab01/index.html /usr/share/nginx/html/index.html
    echo "File copied successfully."
    nginx -g 'daemon off;' &
    tail -f /dev/null
else
    echo "Error: File not found in the volume."
fi

# Add any other commands or logic you might need during container startup

# Finally, start the main process of the container (e.g., start Nginx)
#exec "$@"