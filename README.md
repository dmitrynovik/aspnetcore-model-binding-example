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
and you want to bind it to the request `GET [controller]/[action]/name/John/id/66`

NOTES:
- The binder will skip first 2 path segments which are usually the `[controller]/[action]`
- For every model property, the Url Path is assumed to contain  `... propertyName/propertyValue` adjacent segments e.g. `name/John`
- The property name after extraction from the Url is converted to Pascal case: `name` => `Name`

## Usage
1. Install this package

2. In your ASP.NET controller, change your `action name,  TRequest, TResponse` to whatever is needed:
```

[Route("your action name/{*argv}")]
public ActionResult<TResponse> Search([ModelBinder(typeof(PathModelBinder<TRequest>))] TRequest request)
```

## Supported property types
- System.String
- System.Int16
- System.UInt32
- System.Int32
- System.UInt32
- System.Int64
- System.UInt64

