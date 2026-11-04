# W01 Notes

## 1. Existing pizzas and my additional pizza

I added Pepperoni (id 3) to the seed data in `PizzaService`. Response from `GET /pizza`:

```json
[
  {"id":1,"name":"Classic Italian","isGlutenFree":false},
  {"id":2,"name":"Veggie","isGlutenFree":true},
  {"id":3,"name":"Pepperoni","isGlutenFree":false}
]
```

## 2. API operations

### GET
Request:
```
curl.exe -i http://localhost:5231/pizza
```
Response:
```
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8

[{"id":1,"name":"Classic Italian","isGlutenFree":false},{"id":2,"name":"Veggie","isGlutenFree":true},{"id":3,"name":"Pepperoni","isGlutenFree":false}]
```
Status code: 200 OK

### POST
Request:
```
curl.exe -i -X POST http://localhost:5231/pizza -H "Content-Type: application/json" --data-binary "@post.json"
```
Body (post.json): `{"name":"Hawaiian","isGlutenFree":false}`

Response:
```
HTTP/1.1 201 Created
Content-Type: application/json; charset=utf-8
Date: Wed, 04 Nov 2026 14:07:06 GMT
Server: Kestrel
Location: http://localhost:5231/Pizza/4
Transfer-Encoding: chunked

{"id":4,"name":"Hawaiian","isGlutenFree":false}
```
Status code: 201 Created

### PUT
Request:
```
curl.exe -i -X PUT http://localhost:5231/pizza/2 -H "Content-Type: application/json" --data-binary "@put.json"
```
Body (put.json): `{"id":2,"name":"Veggie Deluxe","isGlutenFree":true}`

Response:
```
HTTP/1.1 204 No Content
Date: Wed, 04 Nov 2026 14:09:44 GMT
Server: Kestrel
```
Status code: 204 No Content

### DELETE
Request:
```
curl.exe -i -X DELETE http://localhost:5231/pizza/1
```
Response:
```
HTTP/1.1 204 No Content
Date: Wed, 04 Nov 2026 14:12:47 GMT
Server: Kestrel
```
Status code: 204 No Content

### Final GET after all operations
```json
[{"id":2,"name":"Veggie Deluxe","isGlutenFree":true},{"id":3,"name":"Pepperoni","isGlutenFree":false},{"id":4,"name":"Hawaiian","isGlutenFree":false}]
```

## 3. Sales summary function

```csharp
void GenerateSalesSummary(IEnumerable<string> salesFiles, string storesDirectory, string outputDirectory)
{
    var report = new System.Text.StringBuilder();
    double grandTotal = 0;
    var details = new System.Text.StringBuilder();

    foreach (var file in salesFiles)
    {
        string salesJson = File.ReadAllText(file);
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
        double fileTotal = data?.Total ?? 0;

        grandTotal += fileTotal;

        string fileName = Path.GetRelativePath(storesDirectory, file);
        details.AppendLine($"  {fileName}: {fileTotal:C}");
    }

    report.AppendLine("Sales Summary");
    report.AppendLine("----------------------------");
    report.AppendLine($" Total Sales: {grandTotal:C}");
    report.AppendLine();
    report.AppendLine(" Details:");
    report.Append(details);

    File.WriteAllText(Path.Combine(outputDirectory, "salesSummary.txt"), report.ToString());
}
```

Sample output (`salesSummary.txt`):
```
Sales Summary
----------------------------
 Total Sales: $2,012.20

 Details:
  sales.json: $88.88
  201\sales.json: $501.22
  202\sales.json: $1,234.22
  203\sales.json: $99.00
  204\sales.json: $88.88

```