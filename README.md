[![Build status](https://ci.appveyor.com/api/projects/status/h2rb5mjk50u2n8hy?svg=true)](https://ci.appveyor.com/project/alexvictoor/mock4net)

# Mock4Net
Mock4Net allows to get an HTTP server in a glance. A fluent API allows to specify the behavior of the server and hence easily stub and mock webservices and REST ressources.
```
server = FluentMockServer.Start();
server
  .Given(
    Requests.WithUrl("/*")
  )
  .RespondWith(
    Responses
      .WithStatusCode(200)
      .WithBody(@"{ msg: ""Hello world!""}")
  );
```

Based on class HttpListener from the .net framework, it is very lightweight and have no external dependencies. 

# Mock4NET API in a nutshell

TBD

# Mock4Net with your favourite test framework

TBD

# Mock4Net as a standalone process

TBD
