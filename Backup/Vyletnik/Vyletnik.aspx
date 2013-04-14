<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Vyletnik.aspx.cs" Inherits="Vyletnik._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">

<head>

<meta name="Description" content="Information architecture, Web Design, Web Standards." />
<meta name="Keywords" content="your, keywords" />
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<meta name="Distribution" content="Global" />
<meta name="Author" content="Michal Svoboda" />
<meta name="Robots" content="index,follow" />

<link rel="stylesheet" href="BrightSide.css" type="text/css" />

<title>Výletník</title>
	
</head>

<body>
<form id="form1" runat="server">
<!-- wrap starts here -->
<div id="wrap">
	
	<div id="header">				
			
		<h1 id="logo">Vý<span class="green">let</span><span class="gray">ník</span></h1>	
		<h2 id="slogan">svobo.net</h2> 
		
			
			
		<!-- Menu Tabs -->
		<ul>
			<li id="current"><a href="Vyletnik.aspx"><span>Domů</span></a></li>
			<li><a href="http://www.svobo.net/misak/"><span>O nás</span></a></li>			
		</ul>	
													
	</div>	
				
	<!-- content-wrap starts here -->
	<div id="content-wrap">													
	<img src="images/headerphoto.jpg" width="820" height="120" alt="headerphoto" class="no-border" />		
		<div id="sidebar" >											
			<h1>Seznam výletů</h1>
            <asp:PlaceHolder ID="placeVylety" runat=server>
              <asp:ListView ID="ListVylety" runat="server">
                    <LayoutTemplate>                    
                       <ul class="sidemenu">
                         <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>  
                       </ul>                  
                    </LayoutTemplate>                         
                    <ItemTemplate>                        
                       <li><a href="#<%# Eval("id") %>"><%# Eval("title") %></a></li>  
                    </ItemTemplate>  
              </asp:ListView>
            </asp:PlaceHolder>		
				
			<h1>Links</h1>
			<ul class="sidemenu">
				<li><a href="http://maps.google.com">Google Maps</a></li>
                <li><a href="http://www.mapy.cz">Mapy CZ</a></li>

			</ul>		
			
			<h1>Moto:</h1>
			<p>Šlapu si, šlapu cestičku. A dokud šlapu, potřebuji mapu.</p>		
				
			<p class="align-right">- Mišák</p>					
		
		</div>
			
		<div id="main">	
			
			<a name="TemplateInfo"></a>
			<h1>Výlety roku <span class="green"><asp:Label runat="server" ID="labelRok"></asp:Label></span></h1>

            <asp:PlaceHolder ID="placeBlog" runat=server>               
                <asp:ListView ID="ListBlog" runat="server">                  
                    <LayoutTemplate>                        
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>                        
                    </LayoutTemplate>                         
                    <ItemTemplate>                                                                                                         
                            <h1 id="<%# Eval("id") %>"><%# Eval("title") %></h1>                           
                            <%# Eval("category") %> <%# Eval("published") %>    
                            <p>
                            <!--                           
                            <a href="ShowVylet.aspx?vylet=<%# Eval("vylet") %>&trip=<%# Eval("trip") %>"><img src="images/globe2.png" align="middle" /></a>-->                            
                            <%# Eval("vyletlink") %>
                            <br />
                            <%# Eval("vyletsilver") %>
                            </p>                                                                                                                                                   
                            <p><%# Eval("content") %><%# Eval("replies") %></p>                        				                                        
                            <br />
                            <%# Eval("map") %>
			                                    
                    </ItemTemplate>                            
                </asp:ListView>	
            </asp:PlaceHolder>						
							
		</div>	
			
		<div id="rightbar">			
			<h1>Tip na výlet</h1>
			<p></p>								
		</div>			
			
	<!-- content-wrap ends here -->		
	</div>

<!-- footer starts here -->	
<div id="footer">
	
	<div class="footer-left">
		<p class="align-left">			
		&copy; 2011 <strong>E+M</strong> |
		Design by <a href="http://www.styleshout.com/">styleshout</a> |
		Valid <a href="http://validator.w3.org/check/referer">XHTML</a> |
		<a href="http://jigsaw.w3.org/css-validator/check/referer">CSS</a>
		</p>		
	</div>
	
	<div class="footer-right">
		<p class="align-right">
		<a href="index.html">Home</a>&nbsp;|&nbsp;
  		<a href="index.html">SiteMap</a>&nbsp;|&nbsp;
   	<a href="index.html">RSS Feed</a>
		</p>
	</div>
	
</div>
<!-- footer ends here -->
	
<!-- wrap ends here -->
</div>

<div style="font-size: 0.8em; text-align: center; margin-top: 1.0em; margin-bottom: 1.0em;">
Design downloaded from <a href="http://www.freewebtemplates.com/">Free Templates</a> - your source for free web templates
</div>
</form>
</body>
</html>

