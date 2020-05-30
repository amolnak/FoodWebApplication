<%@ Page Language="C#" MasterPageFile="~/CPanel/MasterPage.master" AutoEventWireup="true" CodeFile="Admin_AddEdit.aspx.cs" Inherits="CPanel_Admin_AddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" />

        <div class="row">
            <div class="col-md-12 inBodyTop">
                <div class="card">
                    <div class="card-body">
                        <span align="left" id="lblTitle" class="text-dark" runat="server"></span>
                        <div id="dvResetPwd" runat="server" class="pull-right mb-10">
                            <a id="hrefResetPwd" runat="server" class="btn btn-secondary btn-sm" style="color: white;"><i class="fa fa-refresh"></i>&nbsp;Reset Password</a>
                        </div>
                       
                        <div style="clear: both; padding: 10px;"></div>

                        <div class="row">
                            <div class="alert alert-danger p-10 mb-15" id="dvError" runat="server"></div>
                            <div class="form-horizontal form-sm col-lg-12">
                                <div id="dvAdminType" runat="server" class=" row form-group">
                                    <div class="col col-md-3">
                                        <label class="control-label">Admin Type <span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-8 col-md-4">
                                        <asp:DropDownList ID="ddlAdminType" runat="server" CssClass="form-control" TabIndex="1">
                                            <asp:ListItem Text="Select admin type" Selected="True" Value="0" />
                                            <asp:ListItem Text="City Admin" Value="2" />
                                            <asp:ListItem Text="Other Admin" Value="3" />
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlAdminType" ErrorMessage="Please select admin type."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div id="dvCountryddl" runat="server" class="row form-group">
                                    <div class="col col-md-3">
                                        <label class="control-label">City <span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-8 col-md-4">
                                        <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control" TabIndex="2" />
                                        <asp:RequiredFieldValidator InitialValue="0" ID="reqfld_City" runat="server" ControlToValidate="ddlCity" EnableClientScript="true"
                                            ErrorMessage="Please select city." Display="Dynamic" CssClass="text-danger" ValidationGroup="vg"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col col-md-3">
                                        <label class="control-label">Name <span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-8 col-md-4">
                                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Enter name" MaxLength="50" TabIndex="3" />
                                        <asp:RequiredFieldValidator ID="reqfld_Name" runat="server" ControlToValidate="txtName" ErrorMessage="Please enter name"
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col col-md-3">
                                        <label class="control-label">Email ID <span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-10 col-md-6">
                                        <asp:TextBox ID="txtEmailID" runat="server" CssClass="form-control" placeholder="Enter email id" MaxLength="120" TabIndex="4" />
                                        <asp:CustomValidator ID="cstmvld_EmailID" Display="Dynamic" ControlToValidate="txtEmailID" ErrorMessage="Please enter email id."
                                            CssClass="text-danger" ClientValidationFunction="ValidateEmail" runat="server" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col col-md-3">
                                        <label class="control-label">Username <span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-8 col-md-4">
                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" placeholder="Enter username" MaxLength="20" TabIndex="5" />
                                        <asp:RequiredFieldValidator ID="reqfld_Username" runat="server" ControlToValidate="txtUserName" ErrorMessage="Please enter username"
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div id="dvPwd" runat="server" class="row form-group">
                                    <div class="col col-md-3">
                                        <label class="control-label">Password <span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-8 col-md-4">
                                        <div class="input-group frmgrp-group-sm">
                                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter password" oncopy="return false" oncut="return false" onpaste="return false" MaxLength="20" TabIndex="6" />
                                            <span class="input-group-addon" data-placement="left" data-toggle="tooltip" data-original-title="Password must contain: Minimum 8 and Maximum 16 characters atleast 1 Alphabet and 1 Number. Allowed special characters: $@!%*?&">
                                                <i class="fa fa-info-circle"></i>
                                            </span>
                                        </div>

                                        <asp:RequiredFieldValidator ID="reqfld_Pwd" runat="server" ControlToValidate="txtPassword" ErrorMessage="Please enter password"
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />

                                        <asp:RegularExpressionValidator ID="regexp_NewPwd" runat="server" ControlToValidate="txtPassword" Display="Dynamic"
                                            ValidationExpression="^(?=.*[A-Za-z$@$!%*?&])(?=.*\d)[A-Za-z\d$@$!%*?&]{8,16}$" ValidationGroup="vg"
                                            ErrorMessage="Please enter valid password" EnableClientScript="true" CssClass="text-danger" />
                                    </div>
                                </div>
                                <div id="dvCPwd" runat="server" class="row form-group">
                                    <div class="col col-md-3">
                                        <label class="control-label">Confirm Password <span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-8 col-md-4">
                                        <asp:TextBox ID="txtCPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter confirm password" oncopy="return false" oncut="return false" onpaste="return false" MaxLength="20" TabIndex="7" />
                                        <asp:CompareValidator ID="cstmvld_CPwd" runat="server" ControlToValidate="txtPassword" ControlToCompare="txtCPassword" CssClass="text-danger" ValidationGroup="vg"
                                            Display="Dynamic" ErrorMessage="Confirm password should be equal to password" EnableClientScript="true" ToolTip="Confirm password should be equal to password" />
                                    </div>
                                </div>

                                <div class="row form-group">
                                    <div class="col col-md-3">
                                        <label class="control-label">Status <span class="text-danger">*</span></label>
                                    </div>
                                    <div class="col-12 col-md-8">
                                        <div class="radio radio-inline">
                                            <asp:RadioButton ID="rdbActive" runat="server" Text="Active" GroupName="Status" Checked="true" TabIndex="8" />
                                        </div>
                                        <div class="radio radio-inline">
                                            <asp:RadioButton ID="rdbInActive" runat="server" Text="Inactive" GroupName="Status" TabIndex="9" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <hr style="color: #ccc; background-color: #ccc; height: 1px; border: none; clear: both; margin: 10px 0;" />

                        <a href="/Cpanel/AdminList.aspx" class="btn btn-secondary btn-sm" tabindex="11"><i class="fa fa-chevron-left"></i>&nbsp;Back</a>

                        <div class="pull-right" id="dvSubmit">
                            <asp:LinkButton ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" ValidationGroup="vg" CssClass="btn btn-primary btn-sm icon-btnrt"
                                TabIndex="10">Submit&nbsp;<i class="fa fa-thumbs-up"></i></asp:LinkButton>
                        </div>
                        <div class="clear"></div>
                        <asp:CustomValidator ID="custVal" runat="server" OnServerValidate="custVal_ServerValidate"></asp:CustomValidator>
                        <asp:HiddenField ID="hfAdminID" runat="server" />

                    </div>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            function ValidateCity(sender, args) {
                var objCity = $("#<%=ddlCity.ClientID %> option:selected").val();
                if ((objCity == "") || (objCity == "0"))
                    args.IsValid = false;
                else
                    args.IsValid = true;
            }

            function ValidateAdminType(sender, args) {
                var objAdminType = $("#<%=ddlAdminType.ClientID %> option:selected").val();
                if ((objAdminType == "") || (objAdminType == "-1"))
                    args.IsValid = false;
                else
                    args.IsValid = true;
            }

            function ValidateEmail(sender, args) {
       
                var objEmail = $.trim($("#<%=txtEmailID.ClientID %>").val());    
                if ((objEmail == "") || (objEmail == "0")) {
                    $("#<%=cstmvld_EmailID.ClientID %>").html("Please enter email id.");
                    args.IsValid = false;
                }
                else {
                    var dtRegex = new RegExp(/\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/);

                    if (!dtRegex.test(objEmail)) {
                        $("#<%=cstmvld_EmailID.ClientID %>").html("Please enter valid email id.");
                        args.IsValid = false;
                    }
                    else {
                        args.IsValid = true;
                    }
                }
            }
        </script>
    </form>
</asp:Content>
