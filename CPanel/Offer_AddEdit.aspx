<%@ Page Language="C#" MasterPageFile="~/CPanel/MasterPage.master" AutoEventWireup="true" CodeFile="Offer_AddEdit.aspx.cs" Inherits="CPanel_Offer_AddEdit" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" />
        <script type="text/javascript" src="../js/jquery.min.js"></script>
        <script type="text/javascript" src="../js/ckeditor/ckeditor.js"></script>
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
                                    <div class="col-lg-3 col-md-3 col-sm-3 control-label">
                                        Offer Code <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtOfferCode" runat="server" CssClass="form-control" placeholder="Enter offer code." MaxLength="15" TabIndex="1" />
                                        <asp:RequiredFieldValidator ID="reqfld_offercode" runat="server" ControlToValidate="txtOfferCode" ErrorMessage="Please enter offer code."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-3 col-md-3 col-sm-3 control-label">
                                        Offer Name <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtOfferName" runat="server" CssClass="form-control" placeholder="Enter offer name." MaxLength="100" TabIndex="2" />
                                        <asp:RequiredFieldValidator ID="reqfld_offername" runat="server" ControlToValidate="txtOfferName" ErrorMessage="Please enter offer name."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-3 col-md-3 col-sm-3 control-label">
                                        Offer Type 
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtOffertype" runat="server" CssClass="form-control" placeholder="Enter offer type." MaxLength="5" TabIndex="3"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row form-group">
                                    <div class="col-lg-3 col-md-3 col-sm-3 control-label">
                                        Offer Description 
                                    </div>
                                    <div class="col-lg-6 col-md-7 col-sm-8 col-xs-12">
                                        <CKEditor:CKEditorControl name="message" ID="CKeditor1" BasePath="~/scripts/ckeditor/" TabIndex="4" runat="server" Height="150px"></CKEditor:CKEditorControl>

                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-3 col-md-3 col-sm-3 control-label">
                                        Item Name 
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:DropDownList ID="ddlItem" runat="server" CssClass="form-control" TabIndex="5" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-3 col-md-3 col-sm-3 control-label">
                                        Price 
                                    </div>
                                    <div class="col-lg-1 col-md-2 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" placeholder="Enter price." MaxLength="8" TabIndex="6" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-3 col-md-3 col-sm-3 control-label">
                                        Status  
                                    </div>
                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-4">
                                        <div class="radio radio-inline">
                                            <asp:RadioButton ID="rdbActive" runat="server" Text="Active" GroupName="Status" Checked="true" TabIndex="3" />&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:RadioButton ID="rdbInActive" runat="server" Text="Inactive" GroupName="Status" TabIndex="4" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <hr style="color: #ccc; background-color: #ccc; height: 1px; border: none; clear: both; margin: 10px 0;" />

                        <a href="/Cpanel/OfferList.aspx" class="btn btn-secondary btn-sm" tabindex="11"><i class="fa fa-chevron-left"></i>&nbsp;Back</a>

                        <div class="pull-right" id="dvSubmit">
                            <asp:LinkButton ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" ValidationGroup="vg" CausesValidation="true" CssClass="btn btn-primary btn-sm icon-btnrt"
                                TabIndex="12">Submit&nbsp;<i class="fa fa-thumbs-up"></i></asp:LinkButton>
                        </div>
                        <div class="clear"></div>
                        <asp:CustomValidator ID="custVal" runat="server" OnServerValidate="custVal_ServerValidate"></asp:CustomValidator>
                        <asp:HiddenField ID="hfOfferID" runat="server" />

                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
