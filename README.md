# Havit.CastleWindsor.WebForms

Implementation of Castle Windsor DI container to ASP.NET WebForms 4.7.2. In previous versions there is no direct support due to mising extensibility point **HttpRuntime.WebObjectActivator**. You can find more details in [news article](https://blogs.msdn.microsoft.com/dotnet/2018/04/30/announcing-the-net-framework-4-7-2/).

## Instalation to existing project

1. Switch *Target framework* for your project to .NET Framework 4.7.2. If you don't have installed .NET Framework 4.7.2 developer pack. You can download it from [here](https://www.microsoft.com/net/download/thank-you/net472-developer-pack). 

Check web.config and targetFramework in httpRuntime section. Both must be set to 4.7.2.

```xml
<system.web>
	<compilation debug="true" targetFramework="4.7.2"/>
	<httpRuntime targetFramework="4.7.2"/>
</system.web>
```

2. Add nuget package **Havit.CastleWindsor.WebForms**. *Don't forget to reinstall other nuget packges, if you changed Target framework in previous step.*
3. Install nuget packge **Castle.Windsor**.
4. Add this initiaizing code to **Application_Start** method in **Global.asax**. Extension method AddWindsorContainer will initialize new container and will use it for resolving dependencies. 
```c-sharp
IWindsorContainer container = this.AddWindsorContainer();
```
5. Also add using to the header of global.asax
```c-sharp
using Havit.CastleWindsor.WebForms;
```

## Working areas
There are many areas you can use Dependency Injection in WebForms applications now. Here is a complete list:

- Pages and controls
  - WebForms page
  - User control
  - Custom control
- IHttpHandler and IHttpHandlerFactory
- IHttpModule
- Providers
  - BuildProvider
  - ResourceProviderFactory
  - Health monitoring provider
  - Any ProviderBase based provider created by System.Web.Configuration.ProvidersHelper.InstantiateProvider. e.g. custom sessionstate provider

## Known limitations
Because it is not possible to use Dependency Injection through WebOjectActivator in Web Services (*.asmx), we added a workaround for this case, so you can do dependency injection to web services via properties. 

### How to inject to Web Services
1. Web Service must inherit from the abstract class **Havit.CastleWindsor.WebForms.InjectableWebServiceBase**
2. Every property, you want to inject must be marked with the attribute **Havit.CastleWindsor.WebForms.InjectAttribute**
3. Every property, you want to inject must have a **public getter and setter**

## Example 
We have prepared a simple example appliction with one page and one web service with Dependency Injection. See Havit.CastleWindsor.WebForms.Example appliction.

## Troubleshooting
1. If you hit the error, that a page (or an user control) cannot be created because of missing a contructor with zero arguments, check if you had switched your project to .NET FW 4.7.2 (don't forget to check compilation and httpRuntime elements in the web.config file).
