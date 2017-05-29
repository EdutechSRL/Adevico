<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_InputField.ascx.vb" Inherits="Comunita_OnLine.UC_InputField" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayItem" Src="~/Modules/Repository/Common/UC_ModuleRenderAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="InternalFilesUploader" Src="~/Modules/Repository/Common/UC_ModuleInternalUploader.ascx" %>
<asp:MultiView ID="MLVfield" runat="server">
    <asp:View ID="VIWunknown" runat="server"></asp:View>
    <asp:View ID="VIWempty" runat="server"></asp:View>
    <asp:View ID="VIWsingleline" runat="server">
        <div class="fieldobject singleline" runat="server" id="DVsingleline">
            <div class="fieldrow fieldinput" >
                <span class="revisionfield revisioned torevision" id="SPNsinglelineRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBsinglelineRevisionField" runat="server"></asp:Label></label>
                </span>
                <asp:Label runat="server" id="LBsinglelineText" AssociatedControlID="TXBsingleline" CssClass="fieldlabel">Text</asp:Label>
                <div class="fieldrow fielddescription">                
                    <asp:Label runat="server" ID="LBsinglelineDescription" CssClass="description">Description</asp:Label>
                </div>
                <asp:TextBox runat="server" ID="TXBsingleline" CssClass="inputtext"></asp:TextBox>
                <asp:Label runat="server" ID="LBsinglelineHelp" CssClass="inlinetooltip"></asp:Label>     
                <br/>
                <span class="fieldinfo ">
                    <span class="maxchar" runat="server" id="SPNmaxCharsingleline"  Visible="false">
                        <asp:Literal ID="LTmaxCharssingleline" runat="server"></asp:Literal>
                        <span class="availableitems">{available}</span>/<span class="totalitems">{total}</span>
                    </span>
                    <asp:Label ID="LBerrorMessagesingleline" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>        
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWmultiline" runat="server">
        <div class="fieldobject multiline" runat="server" id="DVmultiline">
            <div class="fieldrow fieldinput" >
                <span class="revisionfield revisioned torevision" id="SPNmultilineRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBmultilineRevisionField" runat="server"></asp:Label></label>
                </span>
                <asp:Label runat="server" id="LBmultilineText" AssociatedControlID="TXBmultiline" CssClass="fieldlabel">Text</asp:Label>
                <div class="fielddescription">                
                    <asp:Label runat="server" ID="LBmultilineDescription" CssClass="description">Description</asp:Label>
                </div>
                <asp:TextBox runat="server" ID="TXBmultiline" TextMode="multiline" CssClass="textarea"></asp:TextBox>
                <asp:Label runat="server" ID="LBmultilineHelp" CssClass="inlinetooltip"></asp:Label>                
                <br/>
                <span class="fieldinfo ">
                    <span class="maxchar" runat="server" id="SPNmaxCharmultiline"  Visible="false">
                        <asp:Literal ID="LTmaxCharsmultiline" runat="server"></asp:Literal>
                        <span class="availableitems">{available}</span>/<span class="totalitems">{total}</span>
                    </span>
                    <asp:Label ID="LBerrorMessagemultiline" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span> 
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWdisclaimerStandard" runat="server">
        <div class="fieldobject disclaimer" runat="server" id="DVdisclaimerStandard">
            <div class="fieldrow fieldinput" >
                <span class="revisionfield revisioned torevision" id="SPNdisclaimerStandardRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBdisclaimerStandardRevisionField"  runat="server"></asp:Label></label>
                </span>
                <asp:Label runat="server" id="LBdisclaimerStandardText" CssClass="fieldlabel">Disclaimer</asp:Label>    
                <div class="disclaimerwrapper">
                    <div class="disclaimertext">
                       <asp:Label runat="server" ID="LBdisclaimerStandardDescription">disclaimer text</asp:Label>
                    </div>
                    <div class="disclaimerinput">
                        <asp:RadioButtonList ID="RBLdisclaimer" runat="server" Enabled="false" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="disclaimerlabel">
                            <asp:ListItem Text="Accetta" Value="True" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Rifiuta" Value=""></asp:ListItem>
                        </asp:RadioButtonList>                 
                    </div>                    
                </div>    
                <br/>
                <span class="fieldinfo ">
                    <asp:Label ID="LBerrorMessagedisclaimerStandard" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>                
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWdisclaimerCustomDisplayOnly" runat="server">
        <div class="fieldobject disclaimer custom" runat="server" id="DVdisclaimerCustomDisplayOnly">
            <div class="fieldrow fieldinput" >
                <asp:Label runat="server" id="LBdisclaimerCustomDisplayOnlyText" CssClass="fieldlabel">Disclaimer</asp:Label>    
                <div class="disclaimerwrapper">
                    <div class="disclaimertext">
                       <asp:Label runat="server" ID="LBdisclaimerCustomDisplayOnlyDescription">disclaimer text</asp:Label>
                    </div>
                </div>             
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWdisclaimerCustomSingleOption" runat="server">
        <div class="fieldobject disclaimer custom" runat="server" id="DVdisclaimerCustomSingleOption">
             <div class="fieldrow fieldinput" >
                <span class="revisionfield revisioned torevision" id="SPNdisclaimerCustomSingleOptionFieldRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBdisclaimerCustomSingleOptionFieldRevisionField" runat="server"></asp:Label></label>
                </span>
                <asp:Label runat="server" id="LBdisclaimerCustomSingleOptionText" CssClass="fieldlabel">Disclaimer</asp:Label>    
                <div class="disclaimerwrapper">
                    <div class="disclaimertext">
                       <asp:Label runat="server" ID="LBdisclaimerCustomSingleOptionDescription">disclaimer text</asp:Label>
                    </div>
                    <div class="disclaimerinput">
                        <asp:RadioButtonList ID="RBLsingleOption" runat="server" Enabled="false" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="disclaimerlabel">

                        </asp:RadioButtonList>                           
                        <br/>
                        <span class="fieldinfo ">
                            <span class="minmax" runat="server" id="SPNminMaxCustomSingleOption"  Visible="false">
                                <asp:Literal ID="LTminOptionsCustomSingleOption" runat="server"></asp:Literal>
                                <asp:label ID="LBminOptionCustomSingleOption" CssClass="min" runat="server"></asp:label>
                                <asp:Literal ID="LTmaxOptionsCustomSingleOption" runat="server"></asp:Literal>
                                <asp:label ID="LBmaxOptionCustomSingleOption" CssClass="max" runat="server"></asp:label>
                            </span>
                            <asp:Label ID="LBerrorMessagedisclaimerCustomSingleOption" runat="server" Visible="false" cssClass="generic"></asp:Label>
                        </span>     
                    </div>                    
                </div>               
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWdisclaimerCustomMultiOptions" runat="server">
        <div class="fieldobject disclaimer custom" runat="server" id="DVdisclaimerCustomMultiOptions">
             <div class="fieldrow fieldinput" >
                <span class="revisionfield revisioned torevision" id="SPNdisclaimerCustomMultiOptionsRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBdisclaimerCustomMultiOptionsRevisionField" runat="server"></asp:Label></label>
                </span>
                <asp:Label runat="server" id="LBdisclaimerCustomMultiOptionsText" CssClass="fieldlabel">Disclaimer</asp:Label>    
                <div class="disclaimerwrapper">
                    <div class="disclaimertext">
                       <asp:Label runat="server" ID="LBdisclaimerCustomMultiOptionsDescription">disclaimer text</asp:Label>
                    </div>
                    <div class="disclaimerinput">
                        <asp:CheckBoxList runat="server" CssClass="disclaimerlabel inputcheckboxlist" ID="CBLmultiOptions" RepeatLayout="Flow" RepeatDirection="Horizontal"></asp:CheckBoxList>                   
                    </div>                    
                </div>    
                <br/>
                <span class="fieldinfo ">
                    <span class="minmax" runat="server" id="SPNminMaxCustomMultiOptions"  Visible="false">
                        <asp:Literal ID="LTminOptionsCustomMultiOptions" runat="server"></asp:Literal>
                        <asp:label ID="LBminOptionCustomMultiOptions" CssClass="min" runat="server"></asp:label>
                        <asp:Literal ID="LTmaxOptionsCustomMultiOptions" runat="server"></asp:Literal>
                        <asp:label ID="LBmaxOptionCustomMultiOptions" CssClass="max" runat="server"></asp:label>
                    </span>
                    <asp:Label ID="LBerrorMessagedisclaimerCustomMultiOptions" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>                
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWMail" runat="server">
        <div class="fieldobject disclaimer "  runat="server" id="DVmail">
            <div class="fieldrow fieldinput">
                <span class="revisionfield revisioned torevision" id="SPNmailRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBmailRevisionField" runat="server"></asp:Label></label>
                </span>               
                <asp:Label runat="server" id="LBmailText" AssociatedControlID="TXBmail" CssClass="fieldlabel">Text</asp:Label>
                <div class="fielddescription">                
                    <asp:Label runat="server" ID="LBmailDescription" CssClass="description">Description</asp:Label>
                </div>
                <asp:TextBox runat="server" ID="TXBmail" CssClass="inputtext"></asp:TextBox>
                <asp:RegularExpressionValidator ID="REVmail" runat="server" 
                    ControlToValidate="TXBmail" Enabled="false"
                    ValidationExpression="[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?"></asp:RegularExpressionValidator>
                <asp:Label runat="server" ID="LBmailHelp" CssClass="inlinetooltip"></asp:Label>
                <!--"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"-->
                <br/>
                <span class="fieldinfo ">
                    <span class="maxchar" runat="server" id="SPNmaxCharmail"  Visible="false">
                        <asp:Literal ID="LTmaxCharsmail" runat="server"></asp:Literal>
                        <span class="availableitems">{available}</span>/<span class="totalitems">{total}</span>
                    </span>
                    <asp:Label ID="LBerrorMessagemail" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>                      
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWtelephonenumber" runat="server">
        <div class="fieldobject disclaimer"  runat="server" id="DVtelephonenumber">
            <div class="fieldrow fieldinput">
                <span class="revisionfield revisioned torevision" id="SPNtelephonenumberRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBtelephonenumberRevisionField" runat="server"></asp:Label></label>
                </span>
                <asp:Label runat="server" id="LBtelephonenumberText" AssociatedControlID="TXBtelephonenumber" CssClass="fieldlabel">Text</asp:Label>
                <div class="fielddescription">                
                    <asp:Label runat="server" ID="LBtelephonenumberDescription" CssClass="description">Description</asp:Label>
                </div>
                <asp:TextBox runat="server" ID="TXBtelephonenumber" CssClass="inputtext"></asp:TextBox>
                <asp:RegularExpressionValidator ID="REVTXBtelephonenumber" runat="server" 
                    ControlToValidate="TXBtelephonenumber" Enabled="false"
                    ValidationExpression="((\(\d{3,4}\)|\d{3,4}-)\d{4,9}(-\d{1,5}|\d{0}))|(\d{4,12})"></asp:RegularExpressionValidator>
                <asp:Label runat="server" ID="LBtelephonenumberHelp" CssClass="inlinetooltip"></asp:Label>    
                <br/>
                <span class="fieldinfo ">
                    <span class="maxchar" runat="server" id="SPNmaxChartelephonenumber"  Visible="false">
                        <asp:Literal ID="LTmaxCharstelephonenumber" runat="server"></asp:Literal>
                        <span class="availableitems">{available}</span>/<span class="totalitems">{total}</span>
                    </span>
                    <asp:Label ID="LBerrorMessagetelephonenumber" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>                              
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWradioButtonlist" runat="server">
        <div class="fieldobject radiobuttonlist" runat="server" id="DVradiobuttonlist">
            <div class="fieldrow fieldinput">
                <span class="revisionfield revisioned torevision" id="SPNradiobuttonlistRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBradiobuttonlistRevisionField" runat="server"></asp:Label></label>
                </span>             
                <asp:Label runat="server" ID="LBradioButtonlistText" AssociatedControlID="RBLitems" CssClass="fieldlabel">Items</asp:Label>
                <div class="fielddescription">                
                    <asp:Label runat="server" ID="LBradioButtonlistDescription" CssClass="description">Description</asp:Label>
                </div>
                <asp:RadioButtonList runat="server" CssClass="inputradiobuttonlist" ID="RBLitems" RepeatLayout="Flow"></asp:RadioButtonList>
                <span class="textoption" id="SPNtextOptionRadioButtonList" runat="server" visible="false">
                    <asp:TextBox ID="TXBradiobuttonlist" runat="server" CssClass="inputtext"></asp:TextBox>
                  <%--  <span class="fieldinfo ">
                        <span class="maxchar ">Caratteri disponibili:
                            <span class="availableitems"></span>/<span class="totalitems"></span>
                        </span>
                    </span>--%>
                </span>             
                <asp:Label runat="server" ID="LBradioButtonlistHelp" CssClass="inlinetooltip"></asp:Label>
                <br/>
                <span class="fieldinfo ">
                    <span class="minmax" runat="server" id="SPNminMaxradioButtonlist"  Visible="false">
                        <asp:Literal ID="LTminOptionsradioButtonlist" runat="server"></asp:Literal>
                        <asp:label ID="LBminOptionradioButtonlist" CssClass="min" runat="server"></asp:label>
                        <asp:Literal ID="LTmaxOptionsradioButtonlist" runat="server"></asp:Literal>
                        <asp:label ID="LBmaxOptionradioButtonlist" CssClass="max" runat="server"></asp:label>
                    </span>
                    <asp:Label ID="LBerrorMessageradioButtonlist" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>    
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWcheckboxlist" runat="server">
        <div class="fieldobject checkboxlist" runat="server" id="DVcheckboxlist">
            <div class="fieldrow fieldinput">
                <span class="revisionfield revisioned torevision" id="SPNcheckboxlistRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBcheckboxlistRevisionField" runat="server"></asp:Label></label>
                </span>
                <asp:Label runat="server" ID="LBcheckboxlistText" CssClass="fieldlabel">Items</asp:Label>
                <div class="fielddescription">                
                    <asp:Label runat="server" ID="LBcheckboxlistDescription" CssClass="description">Description</asp:Label>
                </div>
                <span id="SPNcheckboxlist" class="inputcheckboxlist ver" runat="server">
                    <asp:Repeater ID="RPTcheckboxlist" runat="server">
                        <ItemTemplate>
                            <span class="group">
                                <input type="checkbox" runat="server" id="CBoption" value="<%#Container.DataItem.Id %>" />
                                <asp:Label runat="server" ID="LBoptionName" AssociatedControlID="CBoption"><%#Container.DataItem.Name %></asp:Label>
                            </span>
                        </ItemTemplate>
                    </asp:Repeater>
                </span>
                <span class="textoption" id="SPNtextOptionCheckBoxList" runat="server" visible="false">
                    <asp:TextBox ID="TXBcheckboxlist" runat="server" CssClass="inputtext"></asp:TextBox>
                  <%--  <span class="fieldinfo ">
                        <span class="maxchar ">Caratteri disponibili:
                            <span class="availableitems"></span>/<span class="totalitems"></span>
                        </span>
                    </span>--%>
                </span>
                <asp:Label runat="server" ID="LBcheckboxlistHelp" CssClass="inlinetooltip"></asp:Label>

                <br/>
                <span class="fieldinfo ">
                    <span class="minmax" runat="server" id="SPNminMaxcheckboxlist"  Visible="false">
                        <asp:Literal ID="LTminOptionscheckboxlist" runat="server"></asp:Literal>
                        <asp:label ID="LBminOptioncheckboxlist" CssClass="min" runat="server"></asp:label>
                        <asp:Literal ID="LTmaxOptionscheckboxlist" runat="server"></asp:Literal>
                        <asp:label ID="LBmaxOptioncheckboxlist" CssClass="max" runat="server"></asp:label>
                    </span>
                    <asp:Label ID="LBerrorMessagecheckboxlist" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>   
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWdropdownlist" runat="server">
        <div class="fieldobject dropdownlist" runat="server" id="DVdropdownlist">
            <div class="fieldrow fieldinput">
                <span class="revisionfield revisioned torevision" id="SPNdropdownlistRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBdropdownlistRevisionField" runat="server"></asp:Label></label>
                </span>
                <asp:Label runat="server" ID="LBdropdownlistText" AssociatedControlID="DDLitems" CssClass="fieldlabel">Items</asp:Label>
                <div class="fielddescription">                
                    <asp:Label runat="server" ID="LBdropdownlistDescription" CssClass="description">Description</asp:Label>
                </div>
                <asp:DropDownList runat="server" ID="DDLitems"></asp:DropDownList>
                <asp:Label runat="server" ID="LBdropdownlistHelp" CssClass="inlinetooltip"></asp:Label>
                <br/>
                <span class="fieldinfo ">
                    <asp:Label ID="LBerrorMessagedropdownlist" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>   
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWtaxcode" runat="server">
        <div class="fieldobject disclaimer" runat="server" id="DVtaxcode">
            <div class="fieldrow fieldinput"> 
                <span class="revisionfield revisioned torevision" id="SPNtaxcodeRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBtaxcodeRevisionField" runat="server"></asp:Label></label>
                </span>               
                <asp:Label runat="server" id="LBtaxcodeText" AssociatedControlID="TXBtaxcode" CssClass="fieldlabel">Text</asp:Label>
                <div class="fielddescription">                
                    <asp:Label runat="server" ID="LBtaxcodeDescription" CssClass="description">Description</asp:Label>
                </div>
                <asp:TextBox runat="server" ID="TXBtaxcode"  CssClass="inputtext"></asp:TextBox>
                <asp:RegularExpressionValidator ID="REVtaxcode" runat="server" ControlToValidate="TXBtaxcode" Visible="false" Enabled="false"></asp:RegularExpressionValidator>
                <asp:Label runat="server" ID="LBtaxcodeHelp" CssClass="inlinetooltip"></asp:Label>
                <br/>
                <span class="fieldinfo ">
                    <span class="maxchar" runat="server" id="SPNmaxChartaxcode"  Visible="false">
                        <asp:Literal ID="LTmaxCharstaxcode" runat="server"></asp:Literal>
                        <span class="availableitems">{available}</span>/<span class="totalitems">{total}</span>
                    </span>
                    <asp:Label ID="LBerrorMessagetaxcode" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>                            
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWzipcode" runat="server">
        <div class="fieldobject disclaimer" runat="server" id="DVzipcode">
            <div class="fieldrow fieldinput">
                <span class="revisionfield revisioned torevision" id="SPNzipcodeRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBzipcodeRevisionField" runat="server"></asp:Label></label>
                </span>               
                <asp:Label runat="server" id="LBzipcodeText" AssociatedControlID="TXBzipcode" CssClass="fieldlabel">Text</asp:Label>
                <div class="fielddescription">                
                    <asp:Label runat="server" ID="LBzipcodeDescription" CssClass="description">Description</asp:Label>
                </div>
                <asp:TextBox runat="server" ID="TXBzipcode"  CssClass="inputtext"></asp:TextBox>
                <asp:RegularExpressionValidator ID="REVzipcode" runat="server" 
                    ControlToValidate="TXBzipcode" Visible="false" Enabled="false"></asp:RegularExpressionValidator>
                <asp:Label runat="server" ID="LBzipcodeHelp" CssClass="inlinetooltip"></asp:Label>
                <br/>
                <span class="fieldinfo ">
                    <span class="maxchar" runat="server" id="SPNmaxCharzipcode"  Visible="false">
                        <asp:Literal ID="LTmaxCharszipcode" runat="server"></asp:Literal>
                        <span class="availableitems">{available}</span>/<span class="totalitems">{total}</span>
                    </span>
                     <asp:Label ID="LBerrorMessagezipcode" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>                            
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWdatetime" runat="server">
        <div class="fieldobject datetime" runat="server" id="DVdatetime">
            <div class="fieldrow fieldinput">
                <span class="revisionfield revisioned torevision" id="SPNdatetimeRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBdatetimeRevisionField" runat="server"></asp:Label></label>
                </span>
                <asp:Label runat="server" ID="LBdatetimeText" CssClass="fieldlabel">Items</asp:Label>
                <div class="fielddescription">                
                    <asp:Label runat="server" ID="LBdateTimeDescription" CssClass="description">Description</asp:Label>
                </div>
                <telerik:RadDateTimePicker id="RDPdatetime" runat="server"></telerik:RadDateTimePicker>
                <asp:Label runat="server" ID="LBdatetimeHelp" CssClass="inlinetooltip"></asp:Label>
                <br/>
                <span class="fieldinfo ">
                    <asp:Label ID="LBerrorMessagedatetime" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span> 
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWdate" runat="server">
        <div class="fieldobject date"  runat="server" id="DVdate">
            <div class="fieldrow fieldinput">
                <span class="revisionfield revisioned torevision" id="SPNdateRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBdateRevisionField" runat="server"></asp:Label></label>
                </span>
                <asp:Label runat="server" ID="LBdateText"  CssClass="fieldlabel">Items</asp:Label>
                <div class="fielddescription">                
                    <asp:Label runat="server" ID="LBdateDescription" CssClass="description">Description</asp:Label>
                </div>
                <telerik:RadDatePicker ID="RDPdate" runat="server" ></telerik:RadDatePicker>
                <asp:Label runat="server" ID="LBdateHelp" CssClass="inlinetooltip"></asp:Label>
                <br/>
                <span class="fieldinfo ">
                    <asp:Label ID="LBerrorMessagedate" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWtime" runat="server">
        <div class="fieldobject time" runat="server" id="DVtime">
            <div class="fieldrow fieldinput">
                <span class="revisionfield revisioned torevision" id="SPNtimeRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBtimeRevisionField" runat="server"></asp:Label></label>
                </span>
                <asp:Label runat="server" ID="LBTimeText"  CssClass="fieldlabel"></asp:Label>
                <div class="fielddescription">                
                    <asp:Label runat="server" ID="LBtimeDescription" CssClass="description">Description</asp:Label>
                </div>
                <telerik:RadTimePicker ID="RDPtime" runat="server" ></telerik:RadTimePicker>
                <asp:Label runat="server" ID="LBtimeHelp" CssClass="inlinetooltip"></asp:Label>
                <br/>
                <span class="fieldinfo ">
                    <asp:Label ID="LBerrorMessagetime" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWcompanyCode" runat="server">
        <div class="fieldobject singleline" runat="server" id="DVcompanycode">
            <div class="fieldrow fieldinput">
                <span class="revisionfield revisioned torevision" id="SPNcompanycodeRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBcompanycodeRevisionField" runat="server"></asp:Label></label>
                </span>               
                <asp:Label runat="server" id="LBcompanycodeText" AssociatedControlID="TXBcompanycode" CssClass="fieldlabel">Text</asp:Label>
                <div class="fielddescription">                
                    <asp:Label runat="server" ID="LBcompanycodeDescription" CssClass="description">Description</asp:Label>
                </div>
                <asp:TextBox runat="server" ID="TXBcompanycode" CssClass="inputtext"></asp:TextBox>
                <asp:Label runat="server" ID="LBcompanycodeHelp" CssClass="inlinetooltip"></asp:Label>
                <br/>
                <span class="fieldinfo ">
                    <span class="maxchar" runat="server" visible="false" id="SPNmaxCharcompanycode">
                        <asp:Literal ID="LTmaxCharscompanycode" runat="server"></asp:Literal>
                        <span class="availableitems">{available}</span>/<span class="totalitems">{total}</span>
                    </span>
                    <asp:Label ID="LBerrorMessagecompanycode" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>            
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWcompanytaxcode" runat="server">
        <div class="fieldobject singleline" runat="server" id="DVcompanytaxcode">
            <div class="fieldrow fieldinput">
                <span class="revisionfield revisioned torevision" id="SPNcompanytaxcodeRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBcompanytaxcodeRevisionField" runat="server"></asp:Label></label>
                </span>              
                <asp:Label runat="server" id="LBcompanytaxcodeText" AssociatedControlID="TXBcompanytaxcode" CssClass="fieldlabel">Text</asp:Label>
                <div class="fielddescription">                
                    <asp:Label runat="server" ID="LBcompanytaxcodeDescription" CssClass="description">Description</asp:Label>
                </div>
                <asp:TextBox runat="server" ID="TXBcompanytaxcode" CssClass="inputtext"></asp:TextBox>
                <asp:Label runat="server" ID="LBcompanytaxcodeHelp" CssClass="inlinetooltip"></asp:Label>
                <br/>
                <span class="fieldinfo ">
                    <span class="maxchar" runat="server" visible="false" id="SPNmaxCharcompanytaxcode">
                        <asp:Literal ID="LTmaxCharscompanytaxcode" runat="server"></asp:Literal>
                        <span class="availableitems">{available}</span>/<span class="totalitems">{total}</span>
                    </span>
                     <asp:Label ID="LBerrorMessagecompanytaxcode" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>               
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWvatcode" runat="server">
        <div class="fieldobject singleline" runat="server" id="DVvatcode">
            <div class="fieldrow fieldinput">
                <span class="revisionfield revisioned torevision" id="SPNvatcodeRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBvatcodeRevisionField" runat="server"></asp:Label></label>
                </span>              
                <asp:Label runat="server" id="LBvatcodeText" AssociatedControlID="TXBvatcode" CssClass="fieldlabel">Text</asp:Label>
                <div class="fielddescription">                
                    <asp:Label runat="server" ID="LBvatcodeDescription" CssClass="description">Description</asp:Label>
                </div>
                <asp:TextBox runat="server" ID="TXBvatcode" CssClass="inputtext"></asp:TextBox>
                <asp:Label runat="server" ID="LBvatcodeHelp" CssClass="inlinetooltip"></asp:Label>
                <br/>
                <span class="fieldinfo ">
                    <span class="maxchar" runat="server" visible="false" id="SPNmaxCharvatcode">
                        <asp:Literal ID="LTmaxCharsvatcode" runat="server"></asp:Literal>
                        <span class="availableitems">{available}</span>/<span class="totalitems">{total}</span>
                    </span>
                    <asp:Label ID="LBerrorMessagevatcode" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>             
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWname" runat="server">
        <div class="fieldobject singleline"  runat="server" id="DVname">
            <div class="fieldrow fieldinput">
                <span class="revisionfield revisioned torevision" id="SPNnameRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBnameRevisionField" runat="server"></asp:Label></label>
                </span>               
                <asp:Label runat="server" id="LBnameText" AssociatedControlID="TXBname" CssClass="fieldlabel">Text</asp:Label>
                <div class="fielddescription">                
                    <asp:Label runat="server" ID="LBnameDescription" CssClass="description">Description</asp:Label>
                </div>
                <asp:TextBox runat="server" ID="TXBname" CssClass="inputtext"></asp:TextBox>
                <asp:Label runat="server" ID="LBnameHelp" CssClass="inlinetooltip"></asp:Label>
                <br/>
                <span class="fieldinfo ">
                    <span class="maxchar" runat="server" visible="false" id="SPNmaxCharname">
                        <asp:Literal ID="LTmaxCharsname" runat="server"></asp:Literal>
                        <span class="availableitems">{available}</span>/<span class="totalitems">{total}</span>
                    </span>
                    <asp:Label ID="LBerrorMessagename" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>       
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWsurname" runat="server">
        <div class="fieldobject singleline" runat="server" id="DVsurname">
            <div class="fieldrow fieldinput">
                <span class="revisionfield revisioned torevision" id="SPNsurnameRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBsurnameRevisionField" runat="server"></asp:Label></label>
                </span>      
                <asp:Label runat="server" id="LBsurnameText" AssociatedControlID="TXBsurname" CssClass="fieldlabel">Text</asp:Label>
                <div class="fielddescription">                
                    <asp:Label runat="server" ID="LBsurnameDescription" CssClass="description">Description</asp:Label>
                </div>
                <asp:TextBox runat="server" ID="TXBsurname" CssClass="inputtext"></asp:TextBox>
                <asp:Label runat="server" ID="LBsurnameHelp" CssClass="inlinetooltip"></asp:Label>
                <br/>
                <span class="fieldinfo ">
                    <span class="maxchar" runat="server" visible="false" id="SPNmaxCharsurname">
                        <asp:Literal ID="LTmaxCharssurname" runat="server"></asp:Literal>
                        <span class="availableitems">{available}</span>/<span class="totalitems">{total}</span>
                    </span>
                    <asp:Label ID="LBerrorMessagesurname" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>              
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWnote" runat="server">
        <div class="fieldobject note">
            <div class="fieldrow fielddescription">
                <asp:Label runat="server" ID="LBNoteDescription" CssClass="description">Description</asp:Label>
            </div>
        </div>
    </asp:View>
    <asp:View ID="VIWfileinput" runat="server">
        <div class="fieldobject fileupload" runat="server" id="DVfileinput">
            <div class="fieldrow fieldinput">
                <span class="revisionfield revisioned torevision" id="SPNfileinputRevisionField" runat="server" visible="false">
                    <label><asp:Label ID="LBfileinputRevisionField" runat="server"></asp:Label></label>
                </span>              
                <asp:Label runat="server" ID="LBfileinputText" CssClass="fieldlabel">File</asp:Label>
                <div class="fielddescription">
                    <asp:Label runat="server" ID="LBfileinputDescription" CssClass="description">Description</asp:Label>
                </div>
                <CTRL:InternalFilesUploader id="CTRLinternalUploader" runat="server"  DisplayFileSelectLabel="false" MaxFileInput="1" MaxItems="1" DisplayTypeSelector="false"  AjaxEnabled="true"/>
                <asp:Label runat="server" ID="LBfileinputHelp" CssClass="inlinetooltip"></asp:Label>
                <CTRL:DisplayItem ID="CTRLdisplayItem" runat="server" EnableAnchor="true" DisplayExtraInfo="false" DisplayLinkedBy="false" Visible="false"  />
                <span class="icons">
                    <asp:Button ID="BTNremoveFile" runat="server" CssClass="icon delete" />
                </span>
                <br/>
                <span class="fieldinfo ">
                    <asp:Label ID="LBerrorMessagefileinput" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>     
            </div>
        </div>
    </asp:View>
</asp:MultiView>
<asp:Literal ID="LTidField" runat="server" Visible="false"></asp:Literal>
<asp:Literal ID="LTidOption" runat="server" Visible="false"></asp:Literal>
<asp:Literal ID="LTvalueString" runat="server" Visible="false"></asp:Literal>