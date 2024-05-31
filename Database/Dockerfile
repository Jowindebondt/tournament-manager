FROM mcr.microsoft.com/mssql/server:2022-CU12-ubuntu-22.04

ENV currentUser=$USER

# get admin rights
USER root

# Create a config directory
RUN mkdir -p /home/config
WORKDIR /home/config

# Bundle config source
COPY . /home/config

# Grant permissions for to our scripts to be executable
RUN chmod +x /home/config/entrypoint.sh
RUN chmod +x /home/config/configure-db.sh

# return to non-root user
USER ${currentUser}

ENTRYPOINT ["./entrypoint.sh"]