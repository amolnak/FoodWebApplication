<%@ Page Language="C#" MasterPageFile="~/CPanel/MasterPage.master" AutoEventWireup="true" CodeFile="Notification_AddEdit.aspx.cs" Inherits="CPanel_Notification_AddEdit" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .divWaiting {
            position: fixed;
            top: 0;
            left: 0;
            background: #ffffff;
            opacity: 0.8;
            z-index: 10002;
            height: 100%;
            width: 100%;
            padding-top: 20%;
            padding-left: 45%;
        }
    </style>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" />
        <script type="text/javascript" src="../js/jquery.min.js"></script>
        <script type="text/javascript" src="../js/ckeditor/ckeditor.js"></script>
        <script type="text/javascript">
            $(function () {
                CKEDITOR.replace('<%=CKeditor1.ClientID %>', { filebrowserImageUploadUrl: '/CPanel/CkImageUpload.ashx?Form=Notif' });

                var editor = CKEDITOR.instances['CKeditor1'];
                if (editor) { editor.destroy(true); }

                CKEDITOR.config.toolbar_Basic = [['Bold', 'Italic', 'Underline',
                 'Format', 'Font', 'FontSize', '-', 'Undo', 'Redo', '-', 'Link', '-', 'NumberedList', 'BulletedList', '-', 'Outdent', 'Indent']];
                CKEDITOR.config.toolbar = 'Basic';

            });
            CKEDITOR.config.htmlEncodeOutput = true;
            if (!CKEDITOR.env.ie || CKEDITOR.env.version > 7)
                CKEDITOR.env.isCompatible = true;
        </script>
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
                                        Notification Title <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-4 col-md-5 col-sm-5 col-xs-12">
                                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="Enter notification title." MaxLength="150" TabIndex="1" />
                                        <asp:RequiredFieldValidator ID="reqfld_itemname" runat="server" ControlToValidate="txtTitle" ErrorMessage="Please enter notification title."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                        <div class="text-warning f11">Note: For you to identify this notification later.</div>
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <label class="col-lg-3 col-md-3 col-sm-3 control-label">Short Message <span class="text-danger">*</span></label>
                                    <div class="col-lg-4 col-md-5 col-sm-5 col-xs-12">
                                        <asp:TextBox ID="txtShortMsg" runat="server" CssClass="form-control" placeholder="" MaxLength="175" TabIndex="2" Rows="3" TextMode="MultiLine" onblur="return ValidateShortMsg();" onKeyUp="javascript:CountWords(this);" onChange="javascript:CountWords(this);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv_shortMsg" runat="server" ControlToValidate="txtShortMsg" ErrorMessage="Please enter Short Message"
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg"></asp:RequiredFieldValidator>
                                        <span id="cv_txtShortMsg" runat="server" class="text-danger" style="display: none">Invalid Short Message.</span>

                                        <div class="text-warning f11">
                                            Note: This goes as the app notification text. 
                                            <div id="dvWordCnt" runat="server" class="text-danger mt-1 pull-right">175 characters left.</div>
                                        </div>

                                    </div>
                                </div>
                                <div class="row form-group">
                                    <label class="col-lg-3 col-md-3 col-sm-3 control-label">Long Message</label>
                                    <div class="col-lg-6 col-md-7 col-sm-8 col-xs-12">
                                        <CKEditor:CKEditorControl name="message" ID="CKeditor1" BasePath="~/scripts/ckeditor/" TabIndex="3" runat="server" Height="150px"></CKEditor:CKEditorControl>
                                        <div class="text-warning f11">Note: Optional. User can click on the short message and view this if entered.</div>

                                    </div>
                                </div>
                                <div class="row form-group">
                                    <label class="col-lg-3 col-md-3 col-sm-3 control-label hidden-xs"></label>
                                    <div class="col-lg-6 col-md-7 col-sm-8 col-xs-12">
                                        <table class="radio tabletdVT plxxs-0">
                                            <tbody>
                                                <tr>
                                                    <td class="makeTableCell480block">
                                                        <div id="dvSendNow" runat="server" onclick="AskConfirm('SN','dvSendNow');" class="label label-inverse label-icon shadowHover pl-25 pr-10 pt-4 pb-2 plsm-30 ptsm-7 prsm-10 pbsm-6 disInBlock makeTableVT mlxxs-0">
                                                            <input id="rdbSendNow" runat="server" type="radio" clientidmode="static" onclick="AskConfirm('SN', 'dvSendNow');" name="rdbSendOrSchedule" value="SN" tabindex="16" /><label for="ContentPlaceHolder1_rdbSendNow" runat="server">Send Now </label>
                                                        </div>
                                                    </td>
                                                    <td class="pl-10 pt-5 ptsm-8 makeTableCell480block plxxs-0 ptxxs-0">or</td>
                                                    <td class="pl-10 makeTableCell480block plxxs-0">
                                                        <div id="dvSchedule" runat="server" onclick=" AskConfirm('SC', 'dvSchedule');" class="label label-inverse label-icon shadowHover pl-25 pr-10 pt-4 pb-3 plsm-30 ptsm-6 prsm-10 pbsm-5 ptxxs-8 pbxxs-4 disInBlock wauto makeTableVT">
                                                            <input id="rdbSchedule" runat="server" type="radio" clientidmode="static" onclick=" AskConfirm('SC', 'dvSchedule');" name="rdbSendOrSchedule" value="SC" tabindex="16" /><label for="ContentPlaceHolder1_rdbSchedule" runat="server">Schedule </label>
                                                        </div>
                                                        <div style="display: none" id="dvUTCDt" runat="server">
                                                            <asp:TextBox ID="txtDateTime" ClientIDMode="static" runat="server" CssClass="form-control makeTableVT wauto disInBlock" placeholder="Schedule Date Time" MaxLength="50" TabIndex="17" autocomplete="off" onchange="javascript:chkDate();"></asp:TextBox>
                                                            <span class="f12 disInBlock pt-5 pl-5 ptsm-8">(UTC)</span>
                                                        </div>
                                                        <div class="clear"></div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <label class="col-lg-3 col-md-3 col-sm-3 control-label"></label>
                                    <div class="col-lg-4 col-md-3 col-sm-6">
                                        <asp:CustomValidator ID="cv_NptifType" Display="Dynamic" ErrorMessage="Please choose Notification Type: Send Now or Schedule"
                                            CssClass="text-danger" ClientValidationFunction="ValidateNotifType" runat="server" ValidationGroup="vg" />
                                        <asp:CustomValidator ID="cv_scDateTime" Display="Dynamic" ErrorMessage="Please select Schedule Date Time(UTC)."
                                            CssClass="text-danger" ClientValidationFunction="ValidateSchDateTime" runat="server" ValidationGroup="vg" />

                                    </div>
                                </div>
                            </div>
                        </div>
                        <hr style="color: #ccc; background-color: #ccc; height: 1px; border: none; clear: both; margin: 10px 0;" />

                        <a href="NotificationList.aspx" class="btn btn-secondary btn-sm" tabindex="11"><i class="fa fa-chevron-left"></i>&nbsp;Back</a>

                        <div class="pull-right">
                            <asp:UpdateProgress ID="updProgress"
                                AssociatedUpdatePanelID="UpdatePanel3"
                                runat="server">
                                <ProgressTemplate>
                                    <div id="dvLoading" class="divWaiting">
                                        <br />
                                        <span class='text-primary'>Loading  <i class='fa fa-spinner fa-spin'></i></span>
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:LinkButton ID="btnSubmit" runat="server" ValidationGroup="vg" OnClick="btnSubmit_Click" OnClientClick=" return ValidateDetails(event);" CssClass="btn btn-primary btn-sm icon-btnrt" TabIndex="18">Submit<i class="fa fa-thumbs-up"></i></asp:LinkButton>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="clear"></div>
                        <asp:CustomValidator ID="custVal" runat="server" OnServerValidate="custVal_ServerValidate"></asp:CustomValidator>
                        <asp:HiddenField ID="hfNotificationID" runat="server" />
                        <input type="hidden" id="hfNotificType" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript" src="../js/1.11.0/jquery-ui-1.8.min.js"></script>
    <script type="text/javascript" src="../js/bootstrap-datetimepicker.min.js"></script>
    <link href="../css/bootstrap-datetimepicker.min.css" rel="stylesheet" media="all" />
    <link href="../css/jquery.ui.autocomplete.css" rel="stylesheet" media="all" />
    <script type="text/javascript">
        var d = new Date();
        var n = d.toUTCString();
        d.setDate(d.getDate() - 1);
        $("#<%= txtDateTime.ClientID %>").datetimepicker(
                {
                    format: "yyyy-mm-dd hh:ii",
                    startDate: d,
                    minuteStep: 30,
                    pickerPosition: 'top-right'
                });

        $("#<%= txtDateTime.ClientID %>").keydown(function (e) {
            return false;
        });

        function chkDate() {
            var hfNotificType = $("#<%=hfNotificType.ClientID %>").val();
            var txtDate = $("#<%=txtDateTime.ClientID %>");

            if (hfNotificType == "SC" && txtDate.val() == "") {
                $("#<%=cv_scDateTime.ClientID %>").css("display", "block");
                return false;
            }
            else {
                $("#<%=cv_scDateTime.ClientID %>").css("display", "none");
                $("#<%=cv_NptifType.ClientID %>").css("display", "none");
                return true;
            }
        }


        function ValidateNotifType(sender, args) {
            var SN_value = "", SC_value = "";
            if (document.getElementById('<%= rdbSendNow.ClientID%>').checked) {
                SN_value = document.getElementById('<%= rdbSendNow.ClientID%>').value;
            }
            if (document.getElementById('<%=rdbSchedule.ClientID%>').checked) {
                SC_value = document.getElementById('<%=rdbSchedule.ClientID%>').value;
            }

            if (SN_value == "" && SC_value == "") {
                args.IsValid = false;
            }
            else {
                $("#<%=cv_NptifType.ClientID %>").css('display', 'none');
                args.IsValid = true;
            }
        }

        function ValidateSchDateTime(sender, args) {
            var SN_value = "", SC_value = "";
            if (document.getElementById('<%= rdbSendNow.ClientID%>').checked) {
                SN_value = document.getElementById('<%= rdbSendNow.ClientID%>').value;
            }
            if (document.getElementById('<%=rdbSchedule.ClientID%>').checked) {
                SC_value = document.getElementById('<%=rdbSchedule.ClientID%>').value;
            }
            var scDtTime = $("#<%=txtDateTime.ClientID %>").val();
            if (SC_value == "SC" && scDtTime == "" && SN_value == "") {
                args.IsValid = false;
            }
            else {
                $("#<%=cv_scDateTime.ClientID %>").css('display', 'none');
                args.IsValid = true;
            }
        }

        function ValidateString(value) {
            return /^[\p{L}\d\-_\s]+$/u.test(value);
        };

        function isValidStr(str) {
            // return !/[~`!@#$%\^&*()+=\-\[\]\\';,/{}|\\":<>\?]/g.test(str); // original from 
            return !/[~`@#$%\^*+=\-\[\]\\/{}|\\<>\?]/g.test(str);
        }

        function ValidateShortMsg() {
            var txtShortMsg = $("#<%=txtShortMsg.ClientID %>").val().trim();
            var lineFeeds = 0;

            if (txtShortMsg.match(/[^\n]*\n[^\n]*/gi) != null)
                lineFeeds = txtShortMsg.match(/[^\n]*\n[^\n]*/gi).length;

            if ((txtShortMsg.length + lineFeeds) > 175) {
                $("#<%=cv_txtShortMsg.ClientID %>").text("Allowed character limit for Short Message is 175 characters.");
                $("#<%=cv_txtShortMsg.ClientID %>").css('display', '');
                return false;
            }
            else {
                $("#<%=cv_txtShortMsg.ClientID %>").css('display', 'none');
                return true;
            }
        }

        function CountWords(text) {
            var object = document.getElementById(text.id)  //
            var MaxLen = 175;
            var lineFeeds = 0;
            if (object.value.match(/[^\n]*\n[^\n]*/gi) != null)
                lineFeeds = object.value.match(/[^\n]*\n[^\n]*/gi).length;

            if (object.value.trim().length > MaxLen) {
                object.focus(); //set focus to prevent jumping
                object.value = object.value.trim().substring(0, MaxLen - lineFeeds); //truncate the value
                object.scrollTop = object.scrollHeight; //scroll to the end to prevent jumping
                $("#<%= dvWordCnt.ClientID %>").html(MaxLen + " of " + MaxLen);

                $("#<%=cv_txtShortMsg.ClientID %>").text("Allowed character limit for Short Message is 175 characters.");
                $("#<%=cv_txtShortMsg.ClientID %>").css('display', '');
                return false;
            }
            $("#<%=cv_txtShortMsg.ClientID %>").css('display', 'none');
            $("#<%= dvWordCnt.ClientID %>").html((MaxLen - (object.value.trim().length + lineFeeds)) + " characters left.");

            return true;
        }

        (function ($) {
            $.fn.replaceClass = function (pFromClass, pToClass) {
                return this.removeClass(pFromClass).addClass(pToClass);
            };
        }(jQuery));

        function AskConfirm(type, dv) {
            $("#<%=hfNotificType.ClientID %>").val(type);
            if (dv == "dvSchedule") {
                $("#<%=dvUTCDt.ClientID %>").css("display", "inline-block");
                $("#<%=dvSchedule.ClientID %>").replaceClass('label-inverse', 'label-warning');
                $("#<%=dvSendNow.ClientID %>").replaceClass('label-warning', 'label-inverse');

                $("#<%=rdbSchedule.ClientID %>").prop("checked", true);
                $("#<%=rdbSendNow.ClientID %>").prop("checked", false);
                chkDate();
            }
            else {

                $("#<%=dvUTCDt.ClientID %>").css("display", "none");
                $("#<%=dvSendNow.ClientID %>").replaceClass('label-inverse', 'label-warning');
                $("#<%=dvSchedule.ClientID %>").replaceClass('label-warning', 'label-inverse');

                $("#<%=rdbSendNow.ClientID %>").prop("checked", true);
                $("#<%=rdbSchedule.ClientID %>").prop("checked", false);
                chkDate();
            }
        }




    </script>
</asp:Content>
