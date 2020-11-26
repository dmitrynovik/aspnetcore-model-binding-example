# ASP.NET Core Model Binding Demo

## HOWTO Run
1. Clone, compile and run the code
2. Navigate to http://localhost:5001/devices

## HTTP requests example

### Single resource by ID:
E.g. GET /devices/1

### A collection of resources:
GET /devices?Airport=SYD
GET /devices?Airport=SYD&Terminal=2
GET /devices?Airport=SYD&Terminal=2&Type=Unit
... any combination of above
+ supports pagination e.g. /devices?Airport=SYD?page=2&pageSize=10


