<%@ Page Language="C#" MasterPageFile="~/CPanel/MasterPage.master" AutoEventWireup="true" CodeFile="ItemAddOn_AddEdit.aspx.cs" Inherits="CPanel_ItemAddOn_AddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" />
       
        <div class="row">
             <div class="col-md-12 inBodyTop">
                <div class="card">
                    <div class="card-body">
                       <span align="left" id="lblTitle" class="text-dark" runat="server"></span>
                 
                        <div class="row">
                             
                            <div class="alert alert-danger p-10 mb-15" id="dvError" runat="server"></div>
                            <div class="form-horizontal form-sm col-lg-12">
                                <div class="row form-group">
                                    <div class="col col-md-3">
                                        <label class="control-label">AddOn Name<span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-10 col-md-6">
                                        <asp:TextBox ID="txtAddOnName" runat="server" CssClass="form-control" placeholder="Enter addon name." MaxLength="100" TabIndex="1" />
                                        <asp:RequiredFieldValidator ID="reqfld_addonname" runat="server" ControlToValidate="txtAddOnName" ErrorMessage="Please enter addon name."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col col-md-3">
                                        <label class="control-label">AddOn Price <span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-10 col-md-6">
                                        <asp:TextBox ID="txtAddOnPrice" runat="server" CssClass="form-control" placeholder="Enter addon price." MaxLength="8" TabIndex="2" />
                                        <asp:RequiredFieldValidator ID="reqfld_addonprice" runat="server" ControlToValidate="txtAddOnPrice" ErrorMessage="Please enter addon price."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>                               
                            </div>
                        </div>

                        <hr style="color: #ccc; background-color: #ccc; height: 1px; border: none; clear: both; margin: 10px 0;" />

                        <a href="/Cpanel/ItemAddOnList.aspx" class="btn btn-secondary btn-sm" tabindex="11"><i class="fa fa-chevron-left"></i>&nbsp;Back</a>

                        <div class="pull-right" id="dvSubmit">
                            <asp:LinkButton ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" ValidationGroup="vg" CausesValidation="true" CssClass="btn btn-primary btn-sm icon-btnrt"
                                TabIndex="5">Submit&nbsp;<i class="fa fa-thumbs-up"></i></asp:LinkButton>
                        </div>
                        <div class="clear"></div>
                        <asp:CustomValidator ID="custVal" runat="server" OnServerValidate="custVal_ServerValidate"></asp:CustomValidator>
                        <asp:HiddenField ID="hfItemAddOnID" runat="server" />

                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>

