# Long overdue int erubtion

Reproduction for https://github.com/Azure/azure-sdk-for-net/issues/43443 where an `int` is converted to a `long` in TableStorage.

## To Reproduce

1. Clone this repository
1. Open the solution in Visual Studio
1. Start the Azure Functions
1. Call the `GET /api/CreateItem` endpoint
1. Try to get the item with `GET /api/GetItemById/{id}` (will not work)
1. Call the `GET /api/CreateItemWithClient` endpoint
1. Try to get the item with `GET /api/GetItemById/{id}` (will work)

There also is a `GET /api/GetItemByIdWithClient/{id}` endpoint but that does not change the outcome.

## Expected behavior

With the `GET /api/CreateItem` endpoint, the item should be created with an `int` or `Int32` as the `Oops` property.

## Actual behavior

The item is created with an `Int64` as the `Oops` property, and this cannot be read back after creation.

## Workaround

Change the `Oops` property to an `Int64` or `long` in the `OopsEntity` class.

## Environment

- OS: Windows 10

```
dotnet --info
.NET SDK:
 Version:           8.0.204
 Commit:            c338c7548c
 Workload version:  8.0.200-manifests.7d36c14f

Runtime Environment:
 OS Name:     Windows
 OS Version:  10.0.22631
 OS Platform: Windows
 RID:         win-x64
 Base Path:   C:\Program Files\dotnet\sdk\8.0.204\
```
