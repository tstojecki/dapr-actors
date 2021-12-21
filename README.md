## dapr-actors
- net 6
- dapr 1.5.0

## steps

1. `consul agent -dev`
2. `cd web & dotnet run`
3. `cd worker & dotnet run`

use invoke.http to POST 

result: actor missing exception

now stop the services and run worker first, then web, then submit using invoke.http

result: actor responds to all requests
