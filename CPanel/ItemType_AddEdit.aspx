<%@ Page Language="C#" MasterPageFile="~/CPanel/MasterPage.master"  AutoEventWireup="true" CodeFile="ItemType_AddEdit.aspx.cs" Inherits="CPanel_ItemType_AddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" />
       
        <div class="row">
             <div class="col-md-12 inBodyTop">
                <div class="card">
                    <div class="card-body">
                       <div align="center">
                            <span align="left" id="lblTitle" class="text-dark" runat="server"></span>
                        </div>
                 
                        <div class="row mt-20">                             
                            <div class="alert alert-danger p-10 mb-15" id="dvError" runat="server"></div>
                            <div class="form-horizontal form-sm col-lg-12">
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Item Type <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtItemType" runat="server" CssClass="form-control" placeholder="Enter item type" MaxLength="100" TabIndex="1" />
                                        <asp:RequiredFieldValidator ID="reqfld_itemtype" runat="server" ControlToValidate="txtItemType" ErrorMessage="Please enter item type."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>                              
                            </div>
                        </div>

                        <hr style="color: #ccc; background-color: #ccc; height: 1px; border: none; clear: both; margin: 10px 0;" />

                        <a href="/Cpanel/ItemTypeList.aspx" class="btn btn-secondary btn-sm" tabindex="11"><i class="fa fa-chevron-left"></i>&nbsp;Back</a>

                        <div class="pull-right" id="dvSubmit">
                            <asp:LinkButton ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" ValidationGroup="vg" CausesValidation="true" CssClass="btn btn-primary btn-sm icon-btnrt"
                                TabIndex="5">Submit&nbsp;<i class="fa fa-thumbs-up"></i></asp:LinkButton>
                        </div>
                        <div class="clear"></div>
                        <asp:CustomValidator ID="custVal" runat="server" OnServerValidate="custVal_ServerValidate"></asp:CustomValidator>
                        <asp:HiddenField ID="hfItemTypeID" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
