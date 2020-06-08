<%@ Page Language="C#" MasterPageFile="~/CPanel/MasterPage.master" AutoEventWireup="true" CodeFile="Region_AddEdit.aspx.cs" Inherits="CPanel_Region_AddEdit" %>

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
                                        Region Name  <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtRegionName" runat="server" CssClass="form-control" placeholder="Enter region name." MaxLength="100" TabIndex="1" />
                                        <asp:RequiredFieldValidator ID="reqfld_RName" runat="server" ControlToValidate="txtRegionName" ErrorMessage="Please enter region name."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Pincode <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtPincode" runat="server" CssClass="form-control" placeholder="Enter pincode." MaxLength="10" TabIndex="2" />
                                        <asp:RequiredFieldValidator ID="reqfld_Pincode" runat="server" ControlToValidate="txtPincode" ErrorMessage="Please enter pincode."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        City<span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control" TabIndex="3" />
                                        <asp:RequiredFieldValidator ID="reqfld_city" InitialValue="" runat="server" ControlToValidate="ddlCity" ErrorMessage="Please select city."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Status <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-4">
                                        <div class="radio radio-inline">
                                            <asp:RadioButton ID="rdbActive" runat="server" Text="Active" GroupName="Status" Checked="true" TabIndex="4" />&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:RadioButton ID="rdbInActive" runat="server" Text="Inactive" GroupName="Status" TabIndex="5" />
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>

                        <hr style="color: #ccc; background-color: #ccc; height: 1px; border: none; clear: both; margin: 10px 0;" />

                        <a href="/Cpanel/RegionList.aspx" class="btn btn-secondary btn-sm" tabindex="6"><i class="fa fa-chevron-left"></i>&nbsp;Back</a>

                        <div class="pull-right" id="dvSubmit">
                            <asp:LinkButton ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" ValidationGroup="vg" CausesValidation="true" CssClass="btn btn-primary btn-sm icon-btnrt"
                                TabIndex="7">Submit&nbsp;<i class="fa fa-thumbs-up"></i></asp:LinkButton>
                        </div>
                        <div class="clear"></div>
                        <asp:CustomValidator ID="custVal" runat="server" OnServerValidate="custVal_ServerValidate"></asp:CustomValidator>
                        <asp:HiddenField ID="hfRegionID" runat="server" />

                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>

