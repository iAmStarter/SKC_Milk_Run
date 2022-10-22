<%@ Page Title="User Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="SKC_Milk_Run.User" %>

<asp:Content ID="ContentUser" ContentPlaceHolderID="MainContent" runat="server">

    <div class="card card-default" style="width: 98%">
        <div class="card-header">
            <h3 class="card-title">Search filter</h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse">
                    <i class="fas fa-minus"></i>
                </button>
                <button type="button" class="btn btn-tool" data-card-widget="remove">
                    <i class="fas fa-times"></i>
                </button>
            </div>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-sm">
                    <div class="form-group">
                        <label class="FontLabel">User</label>
                        <input type="text" class="form-control" id="Search_User" style="width: 150px; height: 30px">
                    </div>
                </div>
                <div class="col-sm">
                    <div class="form-group">
                        <label class="FontLabel">Supplier</label>
                        <select class="form-control select2" id="Search_Supplier" style="width: 150px; height: 30px">
                        </select>
                    </div>
                </div>
                <div class="col-sm">
                    <div class="form-group">
                        <label class="FontLabel">Name</label>
                        <input type="text" class="form-control" id="Search_Name" style="width: 150px; height: 30px">
                    </div>
                </div>
                <div class="col-sm">
                    <div class="form-group">
                        <label class="FontLabel">User Access</label>
                        <select class="form-control select2" id="Search_User_Access" style="width: 150px; height: 30px">
                        </select>
                    </div>
                </div>
                <div class="col-sm">
                    <div class="form-group">
                        <label class="FontLabel">User Level</label>
                        <select class="form-control select2" id="Search_User_Level" style="width: 150px; height: 30px">
                        </select>
                    </div>
                </div>
                <div class="col-sm">
                    <label class="FontLabel">Status</label>
                    <select class="form-control select2" id="Search_status" style="width: 150px; height: 30px">
                        <option value="">Select</option>
                        <option value="1">Active</option>
                        <option value="0">N/A</option>
                    </select>
                </div>
                <div class="col-sm">
                    <button type="button" id="Search" onclick="SearchUser();" style="width: 100px; height: 30px">Search</button>
                </div>
            </div>
        </div>
    </div>

    <div class="card-body">
        <input type="checkbox" title="Select all" id="selectAllCheckbox" name="selectAllCheckbox" />
        Select All
        <div id="jsGridView"></div>
    </div>
    <div id="detailsDialog">
        <div id="detailsForm">
            <div class="col-sm">
                <input type="hidden" id="HiddenMode" />
                <input type="hidden" id="HiddenID" />
                <label class="FontLabel">Supplier Code</label>
                <select class="form-control select2" id="Supplier_Code" style="width: 100%; height: 30px" name="Supplier_Code">
                </select>
            </div>
            <div class="col-sm">
                <label class="FontLabel">User Name</label>
                <input type="text" class="form-control" id="Username" name="Username">
            </div>
            <div class="col-sm">
                <label class="FontLabel">Password</label>
                <input class="form-control" id="Password" name="Password" type="text">
            </div>
            <div class="col-sm">
                <label class="FontLabel">First Name</label>
                <input type="text" class="form-control" id="First_Name" name="First_Name">
            </div>
            <div class="col-sm">
                <label class="FontLabel">Last Name</label>
                <input type="text" class="form-control" id="Last_Name" name="Last_Name">
            </div>
            <div class="col-sm">
                <label class="FontLabel">Access</label>
                <select class="form-control select2" id="Access" style="width: 100%; height: 30px" name="Access">
                </select>
            </div>
            <div class="col-sm">
                <label class="FontLabel">Level</label>
                <select class="form-control select2" id="Level" style="width: 100%; height: 30px" name="Level">
                </select>
            </div>
            <div class="col-sm">
                <label class="FontLabel">Email</label>
                <input type="email" class="form-control" id="Email" name="Email">
            </div>
            <div class="col-sm">
                <label class="FontLabel">Status</label>
                <select class="form-control select2" id="status" style="width: 100%; height: 30px" name="status">
                    <option value="1">Active</option>
                    <option value="0">N/A</option>
                </select>
            </div>
            <div class="details-form-field" style="margin-top: 15px;">
                <div class="btn-group float-left">
                    <button type="submit" id="save" onclick="DialogSaveData();">Save</button>
                </div>
                <div class="btn-group float-right">
                    <button type="submit" id="Cancel" onclick="$('#detailsDialog').dialog('close');">Cancel</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modal-lg-Gate">
        <div class="modal-dialog modal-sm-customize">
            <div class="modal-content">
                <div class="modal-header">
                    <table id="TabUserGATEHeader" class="table table-bordered table-hover" style="background-color: #ffffff">
                        <thead>
                            <tr>
                                <th colspan="2" class="tablecolheight bgTHTable" style="vertical-align: top;">
                                    <div class="row">
                                        <div class="col-sm">
                                            <span style="font-size: 14px">User : Gate control</span>
                                        </div>
                                        <div class="col-sm">
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true">&times;</span></button>
                                        </div>
                                    </div>
                                </th>
                            </tr>
                            <tr>
                                <th class="tablecolheight bgTHTable" style="vertical-align: middle; text-align: center;">Gate</th>
                                <th class="tablecolheight bgTHTable" style="vertical-align: middle; text-align: center;">
                                    <button id="GateAddAll" type="button" class="btn btn-success" onclick="Add_User_Gate('All');">
                                        <i class="fas fa-plus text-white"></i>Add All
                                    </button>
                                    <button id="GateDelAll" type="button" class="btn btn-danger" onclick="User_Gate_Delete('All')">
                                        <i class="fas fa-trash"></i>Delete All
                                    </button>
                                </th>
                            </tr>
                            <tr>
                                <th class="tablecolheight">
                                    <div class="input-group" id="GateCodeselect" data-target-input="nearest">
                                        <select class="form-control select2" id="GateCode" style="width: 100%;">
                                        </select>
                                    </div>
                                </th>
                                <th style="text-align: center">
                                    <input type="hidden" id="UserId" />
                                    <button id="GateAdd" type="button" class="btn btn-success" onclick="Add_User_Gate('');">
                                        <i class="fas fa-plus text-white"></i>
                                    </button>
                                    <button id="GateDel" type="button" class="btn btn-danger" onclick="User_Gate_Delete('')">
                                        <i class="fas fa-trash"></i>
                                    </button>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <div class="modal-body" style="max-height: 600px; overflow-y: auto;">
                    <table id="TabUserGATE" class="table table-bordered table-hover" style="background-color: #ffffff">
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <!-- /.modal -->

    <script src="dhtmlxgantt/dhtmlxgantt.js?v=7.0.13"></script>
    <link rel="stylesheet" href="dhtmlxgantt/dhtmlxgantt.css?v=7.0.13">
    <script src="GlobalJavascript.js?v=<%=yyMMddHHmmss%>"></script>
    <style>
        .bgTHTable {
            background-color: #f9f9f9;
        }

        .bgTHTableAdd {
            background-color: #e3fbdd;
        }

        .tablecolheightAdd {
            height: 10px !important;
        }

        .modal-sm-customize {
            max-width: 300px;
        }

        .modal-xl-View {
            width: calc(100%-100px) !important;
            max-width: calc(100%-100px) !important;
        }
    </style>
    <script type="text/javascript">
        var Path = "<%=ResolveUrl("~/WebService.asmx")%>";
        var usr_id = "<%=UserId%>";
        var user_name = "<%=user_name%>";
        var UserLevel = "<%=UserLevel%>";
        var supplier_code = "<%=supplier_code%>";

        var dateNow = new Date();

        var status_edit_ = "0";

        $('#Search_User_Access').select2({
            placeholder: "Please select",
            allowClear: true // This is for clear get the clear button if wanted
        });

        $('#Access').select2({
            placeholder: "Please select",
            allowClear: true // This is for clear get the clear button if wanted
        });


        $('#Search_Supplier').select2({
            placeholder: "Please select",
            allowClear: true // This is for clear get the clear button if wanted
        });

        $('#Supplier_Code').select2({
            placeholder: "Please select",
            allowClear: true // This is for clear get the clear button if wanted
        });

        $('#Level').select2({
            placeholder: "Please select",
            allowClear: true // This is for clear get the clear button if wanted
        });

        $('#Access').select2({
            placeholder: "Please select",
            allowClear: true // This is for clear get the clear button if wanted
        });

        // SKC's user (Level 2,3)
        if (supplier_code == "SKC") {
            if (UserLevel != "1") {
                status_edit_ = "SKC";
            }
        }
        else { // Supplier Level 2
            if (UserLevel != "1") {
                status_edit_ = "2";
            }
        }

        GetSupplierlist("Search_Supplier", "L", UserLevel, supplier_code, usr_id);

        GetSupplierlist("Supplier_Code", "L", UserLevel, supplier_code, usr_id);

        GetUser_Level("Search_User_Level", UserLevel, "S");

        GetUser_Access("Search_User_Access", UserLevel, supplier_code, "S");

        GetUser_Level("Level", UserLevel, "");

        GetUser_Access("Access", UserLevel, supplier_code, "");

        //Initialize Select2 Elements
        $('.select2').select2()

        //Initialize Select2 Elements
        $('.select2bs4').select2({
            theme: 'bootstrap4'
        })

        function ClearFilter(Form) {
            $(Form).find("input:not(:disabled),select:not(:disabled)").val(null);
        }

        function SearchUser() {
            $('#jsGridView').jsGrid('loadData');
        }

        $("#jsGridView").jsGrid({

            height: "auto",
            width: "98%",
            sorting: true,
            paging: true,
            autoload: true,
            pageSize: 17,

            controller: {
                loadData: function (filter) {

                    var Search_User = $("#Search_User").val();
                    var Search_Supplier = $("#Search_Supplier").val();
                    var Search_Name = $("#Search_Name").val();
                    var Search_status_ = $("#Search_status").val();
                    var Search_User_Level = $("#Search_User_Level").val();
                    var Search_User_Access = $("#Search_User_Access").val();

                    var sql_ = Supplier_sql(UserLevel, supplier_code, user_name, "");

                    var data = $.Deferred();

                    $.ajax({
                        type: "POST"
                        , url: Path + "/GetUser"
                        , data: {
                            "Username": Search_User,
                            "Name": Search_Name,
                            "SupplierCode": Search_Supplier,
                            "UserLevel": Search_User_Level,
                            "AccessLevel": Search_User_Access,
                            "Status_": Search_status_
                            , "sql_supplier": sql_
                        },
                        dataType: "json"
                    }).done(function (response) {

                        data.resolve(response);

                    });
                    return data.promise();
                }
            },
            fields: [
                {
                    headerTemplate: function () {
                        return $("<button style='background-color:#ff0000;border:0px;color:#ffffff;width:45px'>").attr("type", "button").text("Delete")
                            .on("click", function () {
                                deleteSelectedItems();
                            });
                    },
                    itemTemplate: function (_, item) {
                        return $("<input id='CbID' value='" + item.UserId + "'>").attr("type", "checkbox").attr("class", "CbID")
                            .prop("checked", $.inArray(item, selectedItems) > -1)
                            .on("change", function () {
                                $(this).is(":checked") ? selectItem(item) : unselectItem(item);
                            });
                    },
                    align: "center",
                    width: 50
                },
                { name: "Username", title: "User Name", type: "text", width: 80 },
                { name: "Password_txt", title: "Password", type: "text", width: 50 },
                { name: "FirstName", title: "First Name", type: "text", width: 120 },
                { name: "LastName", title: "Last Name", type: "text", width: 120 },
                { name: "SupplierCode", title: "Supplier Code", type: "text", width: 80 },
                { name: "AccessName", title: "Access Level", type: "text", width: 80 },
                { name: "LevelName", title: "User Level", type: "text", width: 80 },
                { name: "Email", title: "Email", type: "text", width: 80 },
                { name: "Status", type: "select", items: [{ Name: "Active", Id: "1" }, { Name: "N/A", Id: "0" }], valueField: "Id", textField: "Name", width: 40 },
                {
                    itemTemplate: function (_, item) {
                        if (status_edit_ == "SKC") {
                            return $("<button id='btnGateID" + item.UserId + "' value='" + item.UserId + "' data-toggle='modal' data-target='#modal-lg-Gate' onclick =\"Get_Data_Gate('" + item.UserId + "')\">").attr("type", "button").text("Gate Control")
                                .on("click", function () {
                                    document.getElementById("UserId").value = item.UserId;
                                    GetGate();
                                });
                        }
                        else {
                            return $("<button id='btnGateID" + item.UserId + "' value='" + item.UserId + "'>").attr("type", "button").text("Gate Control").attr('disabled', 'disabled');
                        }
                    }, width: 60, align: "center"
                },
                {
                    headerTemplate: function () {
                        return $("<button style='background-color:#00cc66;border:0px;color:#ffffff;width:45px'>").attr("type", "button").text("Add")
                            .on("click", function () {
                                document.getElementById("HiddenMode").value = "I";
                                document.getElementById("HiddenID").value = "";
                                showDetailsDialog("Add", {});
                            });
                    },
                    itemTemplate: function (_, item) {
                        return $("<button id='btnID" + item.UserId + "' value='" + item.UserId + "'>").attr("type", "button").text("Edit")
                            .on("click", function () {

                                document.getElementById("HiddenMode").value = "E";
                                document.getElementById("HiddenID").value = item.UserId;

                                showDetailsDialog("Edit", item);
                            });
                    }, width: 30, align: "center"
                }
            ]

        });

        var selectedItems = [];

        var selectItem = function (item) {
            selectedItems.push(item);
            if ($(".CbID").length == $(".CbID:checked").length) {
                $("#selectAllCheckbox").prop("checked", true);
            } else {
                $("#selectAllCheckbox").prop("checked", false);
            }
        };

        var unselectItem = function (item) {
            selectedItems = $.grep(selectedItems, function (i) {
                return i !== item;
            });
            if (selectedItems.length == 0) {
                $('#selectAllCheckbox').attr('checked', false);
            }
            if ($(".CbID").length == $(".CbID:checked").length) {
                $("#selectAllCheckbox").prop("checked", true);
            } else {
                $("#selectAllCheckbox").prop("checked", false);
            }
        };

        $("#selectAllCheckbox").click(function (item) {
            selectedItems = [];
            if (this.checked) { // check select status
                $('.CbID').each(function () {
                    this.checked = true;
                    selectItem({ ID: $(this).val() });
                });
            } else {

                $('.CbID').each(function () {
                    this.checked = false;
                    unselectItem(item);
                });
                selectedItems = [];
            }
        });

        var deleteSelectedItems = function () {
            if (!selectedItems.length || !confirm("Are you sure?"))
                return;

            deleteClientsFromDb(selectedItems);

            var $grid = $("#jsGridView");
            $grid.jsGrid("option", "pageIndex", 1);
            $grid.jsGrid("loadData");

            selectedItems = [];
        };

        var deleteClientsFromDb = function (deletingClients) {

            for (let i = 0; i < deletingClients.length; i++) {
                DeleteData(deletingClients[i].UserId);
            }
        };

        $("#detailsDialog").dialog({
            autoOpen: false,
            width: 400,
            close: function () {
                //Reset Form
                $("#detailsForm").find(".error").removeClass("error");
            }
        });

        var formSubmitHandler = $.noop;

        var showDetailsDialog = function (dialogType, client) {

            $("#Supplier_Code").val(client.SupplierCode).trigger('change');
            $("#Username").val(client.Username);
            $("#Password").val(client.Password);
            $("#First_Name").val(client.FirstName);
            $("#Last_Name").val(client.LastName);

            $("#Access").val(client.AccessLevel).trigger('change');
            $("#Level").val(client.UserLevel).trigger('change');
            $("#Email").val(client.Email).trigger('change');

            if (dialogType != "Add") {
                $("#status").val(client.Status).trigger('change');
                $("#Username").attr('readonly', true);
            }
            else {
                $("#status").val("1").trigger('change');
                $("#Access").prop("selectedIndex", 0).trigger('change');
                $("#Level").prop("selectedIndex", 0).trigger('change');
                $("#Username").attr('readonly', false);
            }
            $("#detailsDialog").dialog("option", "title", dialogType + " Client")
                .dialog("open");
        };

        function DialogSaveData() {

            if ($("#Username").val().trim() != "") {

                if ($("#Password").val().trim() != "") {

                    if ($("#Supplier_Code").val().trim() != "") {

                        var Mode_Ins_Edt = document.getElementById("HiddenMode").value;

                        var Id_Ins_Edt = document.getElementById("HiddenID").value;

                        var vAccess = $("#Access").val() == undefined ? 'Null' : $("#Access").val().trim();
                        vAccess = vAccess.replace(/ /g, '%20');

                        if (Mode_Ins_Edt == "I") {

                            $.ajax({
                                method: "POST"
                                , url: Path + "/Check_User_Data"
                                , data: { "User_name": $("#Username").val().trim() }
                                , dataType: "json"
                                , cache: false
                                , async: true

                                , success: function (response) {

                                    let data = eval(response);

                                    if (data.length <= 0) {

                                        User_Save("I", "", $("#Username").val().trim(), $("#Password").val().trim(), $("#First_Name").val().trim(), $("#Last_Name").val().trim()
                                            , $("#Supplier_Code").val().trim(), vAccess, $("#Level").val().trim(), $("#status").val().trim(), $("#Email").val().trim());
                                    }
                                    else {
                                        Alert("User name already exist !");
                                    }
                                }
                                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    Alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                                }
                                , timeout: 1600
                                , complete: function (XMLHttpRequest, textStatus) {
                                    window.setTimeout(function () {
                                    }, 1600);
                                }
                            });

                        }
                        else {

                            User_Save("E", Id_Ins_Edt, $("#Username").val().trim(), $("#Password").val().trim(), $("#First_Name").val().trim(), $("#Last_Name").val().trim()
                                , $("#Supplier_Code").val().trim(), vAccess, $("#Level").val().trim(), $("#status").val().trim(), $("#Email").val().trim());
                        }
                    }
                    else {
                        Alert("Please input Supplier Code !");
                    }
                }
                else {
                    Alert("Please input Password !");
                }
            }
            else {
                Alert("Please input User name !");
            }
        }

        function DeleteData(Id_) {

            if (Id_ != "") {
                User_Save("D", Id_, "", "", "", "", "", "", "", "","")
            }
            else {
                Alert("Please select delete data !");
            }

        }

        function User_Save(Mode_, Id_, User_name, Password, FirstName, LastName, SupplierCode, AccessLevel, UserLevel, Status_,Email_) {

            var Sql_ = "";

            if (Status_ == "") {
                Status_ = "1";
            }

            if (Mode_ == "I") {
                Sql_ = " INSERT INTO [dbo].[Data_User](Username,Password,FirstName,LastName,SupplierCode,AccessLevel,UserLevel,[Status],Email)"
                Sql_ += " VALUES('" + User_name + "','" + Password + "','" + FirstName + "','" + LastName + "','" + SupplierCode + "'," + AccessLevel + "," + UserLevel + ", '" + Status_ + "','" + Email_+"')";
            }
            else if (Mode_ == "E") {
                Sql_ = " UPDATE [dbo].[Data_User] SET Password = '" + Password + "',FirstName = '" + FirstName + "',LastName = '" + LastName + "',SupplierCode = '" + SupplierCode + "',Status = '" + Status_ + "'"
                Sql_ += " , AccessLevel = " + AccessLevel + ",UserLevel = " + UserLevel + ", Email ='" + Email_+"'  WHERE [UserId] =" + Id_;
            }
            else {
                Sql_ = " DELETE FROM [dbo].[Data_User] WHERE [UserId] =" + Id_;
            }


            $.ajax({
                method: "POST"
                , url: Path + "/Update_Table_Data"
                , data: {
                    "SQL": Sql_
                }
                , dataType: "json"
                , cache: false
                , beforeSend: function () {
                }
                , success: function (response) {
                    let data = eval(response);

                    if (data['0'].Status == "Y") {
                        Completed("Update data completed ! ");
                        if (Mode_ != "D") {
                            $('#detailsDialog').dialog('close');
                        }
                    }
                    else {
                        Alert("Error : " + data['0'].Message);
                    }
                }
                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                    Alert("Error : " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                }
                , complete: function (XMLHttpRequest, textStatus) {
                    window.setTimeout(function () {
                        if (Mode_ != "D") {

                            $("#jsGridView").jsGrid("loadData");
                        }
                    }, 1600);
                }
                , timeout: 6000
            });
        }

        function Get_Data_Gate(Id_) {

            ResetTable("TabUserGATE");

            document.getElementById("UserId").value = Id_;

            $.ajax({
                method: "POST"
                , url: Path + "/GetUserGate"
                , data: { "Id_": Id_ }
                , dataType: "json"
                , cache: false
                , async: true

                , success: function (response) {
                    let data = eval(response);

                    let i = 0;

                    for (let i = 0; i < data.length; i++) {

                        let v = data[i];

                        let tb = document.getElementById("TabUserGATE");
                        let tbody = tb.querySelector("tbody");

                        let cols = "<td colspan=\"2\" style=\"vertical-align: middle\; \"><input type='checkbox' id='" + v.Gate_Id + "'> <label class='FontLabel' id='lblGateCode" + v.Gate_Id + "'>" + v.Gate_No + "</label></td>";

                        let tr = document.createElement("tr");
                        tr.innerHTML = cols;

                        tbody.appendChild(tr);

                        continue;
                    }
                }
                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                    Alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                }
                , complete: function (XMLHttpRequest, textStatus) {
                    window.setTimeout(function () {
                        //// 
                    }, 0);
                }
                , timeout: 6000
            });
        }

        function GetGate() {

            let select_GateCode = document.getElementById("GateCode");
            var user_ = document.getElementById("UserId").value;
            var options = "";

            removeOptions_select2("GateCode");

            options = '<option value="">Select</option>';
            select_GateCode.innerHTML = select_GateCode.innerHTML + options;

            $.ajax({
                method: "POST"
                , url: Path + "/GetGate_UserGate"
                , data: { "Code": "", "UserId": user_ }
                , dataType: "json"
                , cache: false
                , async: true

                , success: function (response) {
                    let data = eval(response);

                    let i = 0;

                    for (let i = 0; i < data.length; i++) {
                        let v = data[i];

                        options = '<option value="' + v.Id + '" >' + v.Gate_No + '</option>';
                        select_GateCode.innerHTML = select_GateCode.innerHTML + options;
                        continue;

                    }

                }
                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                    Alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                }
                , timeout: 0
            });

        }

        function Add_User_Gate(mode_) {

            var user_ = document.getElementById("UserId").value;
            var GateCode_ = $("#GateCode").val();

            // Date Update 
            if (mode_ == "All") {
                User_Gate_Save("I_All", user_, GateCode_);
            }
            else {
                if (GateCode_ != "") {
                    User_Gate_Save("I", user_, GateCode_);
                }
                else {
                    Alert('Please select "Gate" !');
                }
            }

        }

        function User_Gate_Delete(mode_) {

            var user_ = document.getElementById("UserId").value;

            // Date Update 
            if (mode_ == "All") {
                User_Gate_Save("D_All", user_, "");
            }
            else {
                User_Gate_deleteMoreRows("TabUserGATE");
            }
        }

        function User_Gate_deleteMoreRows(tableID) {
            var user_ = document.getElementById("UserId").value;
            var table = document.getElementById(tableID);
            var rowCount = table.rows.length;
            var selectedRows = getCheckedBoxes();
            var id_ = "";
            selectedRows.forEach(function (currentValue) {
                if (id_ == "") {
                    id_ = currentValue.id.toString();
                }
                else {
                    id_ += "," + currentValue.id.toString();
                }
            });

            User_Gate_Save("D", user_, id_);
        }
        function User_Gate_Save(Mode_, Userid_, GateId_) {

            var Sql_ = "";

            if (Mode_ == "I") {
                Sql_ = " INSERT INTO [dbo].[Tbl_User_Gate](UserId, Gate_Id) VALUES (" + Userid_ + ", " + GateId_ + ")";
            }
            else if (Mode_ == "D") {
                Sql_ = " DELETE FROM [dbo].[Tbl_User_Gate] WHERE UserId =" + Userid_ + " AND Gate_Id in (" + GateId_ + ")";
            }
            else if (Mode_ == "D_All") {
                Sql_ = " DELETE FROM [dbo].[Tbl_User_Gate] WHERE UserId =" + Userid_;
            }
            else if (Mode_ == "I_All") {
                Sql_ = " INSERT INTO [dbo].[Tbl_User_Gate](UserId, Gate_Id) SELECT " + Userid_ + ",[Id] FROM [dbo].[Tbl_Gate] Where Id not in (select Gate_Id FROM [dbo].[Tbl_User_Gate] WHERE UserId = " + Userid_ + ")";
            }

            $.ajax({
                method: "POST"
                , url: Path + "/Update_Table_Data"
                , data: {
                    "SQL": Sql_
                }
                , dataType: "json"
                , cache: false
                , beforeSend: function () {
                }
                , success: function (response) {
                    let data = eval(response);

                    if (data['0'].Status == "Y") {
                        Completed("Update data completed ! ");
                    }
                    else {
                        Alert("Error : " + data['0'].Message);
                    }
                }
                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                    Alert("Error : " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                }
                , complete: function (XMLHttpRequest, textStatus) {
                    window.setTimeout(function () {
                        Get_Data_Gate(Userid_);
                        GetGate();
                        $("#modal-lg-Gate").modal('show');
                    }, 0);
                }
                , timeout: 6000
            });
        }


    </script>
</asp:Content>
