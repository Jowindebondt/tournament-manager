#!/bin/bash

# Start the script to create the DB and user
/home/config/configure-db.sh &

# Start SQL Server
/opt/mssql/bin/sqlservr