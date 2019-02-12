<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Default.aspx.cs" Inherits="Havit.CastleWindsor.WebForms.Example.DefaultPage" %>
<%@ Register Src="~/DemoControl.ascx" TagPrefix="havit" TagName="DemoControl" %>

<html>
    <head>
        <title>Hello Havit.CastleWindsor.WebForms!</title>
    </head>
    <body>
		<div>Demo1: <havit:DemoControl runat="server" /></div>
		<div>Demo2: <havit:DemoControl runat="server" /></div>
    </body>
</html>
