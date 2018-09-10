#!/bin/bash
export ASPNETCORE_ENVIRONMENT=local
cd src/DShop.Services.Operations
dotnet run --no-restore