# ASP.NET Core Url Path Model Binder

## Purpose
To bind custom types to the path part of request URL

## Example
Imagine you have a custom model type with properties `Id` and `Name`:

```
public class Person
{
	public int Id { get; set;}
	public string Name { get; set;}
}

```
and you want to bind it to the request `GET [controller]/[action]/Name/John/Id/66`

## Usage
```
[Route("<your action name>/{*argv}")]
public ActionResult<your response type>> Search([ModelBinder(typeof(PathModelBinder<your request model type>))] DeviceRequest request)
```

## Supported property types
* System.String
* System.Int16
* System.UInt32
* System.Int32
* System.UInt32
* System.Int64
* System.UInt64

## HOWTO Run ASP.NET Web example project
1. Clone, compile and run the code
2. Navigate to http://localhost:5001/
3. Try the following URLs:
http://localhost:5001/Aiport/SYD
http://localhost:5001/Aiport/SYD/Terminal/1



