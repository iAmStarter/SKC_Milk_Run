<%@ Page Title="Permission" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Permission.aspx.cs" Inherits="SKC_Milk_Run.Permission" %>

<asp:Content ID="ContentPermission" ContentPlaceHolderID="MainContent" runat="server">    
    <div class="card card-default" style="width:98%">
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
        <div class="card-body"  style="width:800px" >
            <div class="row">
                <div class="col-sm">
                    <div class="form-group">
                        <label class="FontLabel">Access Name</label>
                        <input type="text" class="form-control" id="Search_AccessName" style="width: 140px; height: 30px">
                    </div>
                </div>
                <div class="col-sm">
                    <div class="form-group">
                        <label class="FontLabel">Admin mode</label>
                        <select class="form-control select2" id="Search_Admin" style="width: 120px; height: 30px">
                        <option value="">Select</option>
                        <option value="1">Admin Only</option>
                        <option value="0">All User</option>
                    </select>
                </div></div>
                <div class="col-sm">
                    <button type="button" id="Search" onclick="SearchUserAccess();" style="width: 100px; height: 30px">Search</button>
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
                <label class="FontLabel">Access Name</label>
                <input type="text" class="form-control" id="Access_Name" name="Access_Name">
            </div> 
            <div class="col-sm">
                <label class="FontLabel">Admin mode</label>
                 <select class="form-control select2" id="Admin_mode" style="width: 100%;height:30px" name="Admin_mode">
                   <option value="1">Admin Only</option>
                   <option value="0">All User</option>
                </select>
            </div>
            <div class="details-form-field" style="margin-top: 15px;">
                 <div class="btn-group float-left" >
                 <button type="submit" id="save" onclick="DialogSaveData();">Save</button></div>
                  <div class="btn-group float-right" ><button type="submit" id="Cancel" onclick="$('#detailsDialog').dialog('close');">Cancel</button></div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modal-lg-Object">
        <div class="modal-dialog modal-sm-customize">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">User Access Menu(Object)</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <table id="TabConfig_Access" class="table table-bordered table-hover" style="background-color: #ffffff">
                        <thead>
                            <tr>
                                <th class="tablecolheight bgTHTable" style="vertical-align: middle; text-align: center; width: 150px">Menu(Object)</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                        <tfoot>
                            <tr>
                                <th class="tablecolheight">
                                    <div class="input-group" id="ObjectMenuselect" data-target-input="nearest">
                                        <select class="form-control select2" id="ObjectMenu" style="width: 100%;">
                                        </select>
                                    </div>
                                </th>
                                <th style="text-align: center">
                                     <input type="hidden" id="AccessID" />
                                    <button id="AccessAdd" type="button" class="btn btn-success" onclick="Add_Access();">
                                        <i class="fas fa-plus text-white"></i>
                                    </button>
                                </th>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <script src="dhtmlxgantt/dhtmlxgantt.js?v=7.0.13"></script>
    <link rel="stylesheet" href="dhtmlxgantt/dhtmlxgantt.css?v=7.0.13">
    <script src="GlobalJavascript.js?v=<%=yyMMddHHmmss%>"></script>    
    <style>        
        .bgTHTable {
            background-color: #f9f9f9;
        }
        .modal-sm-customize {
            max-width: 300px;
        }
        .modal-xl-View {
            width: calc(100%-100px) !important;
            max-width : calc(100%-100px) !important;
        }        
    </style> 
    <script type="text/javascript">
        var Path = "<%=ResolveUrl("~/WebService.asmx")%>";
        var usr_id = "<%=UserId%>";
        var user_name = "<%=user_name%>";
        var UserLevel = "<%=UserLevel%>";
        var supplier_code = "<%=supplier_code%>";

        var dateNow = new Date();

        //Initialize Select2 Elements
        $('.select2').select2()

        //Initialize Select2 Elements
        $('.select2bs4').select2({
            theme: 'bootstrap4'
        })

        //function Alert(msg) {

        //    toastr.error('<label style="font-size:14px;">' + msg + '</label>');
        //}

        //function Completed(msg) {

        //    toastr.success('<label style="font-size:14px;">' + msg + '</label>');
        //}

        function ClearFilter(Form) {
            $(Form).find("input:not(:disabled),select:not(:disabled)").val(null);
        }

        function SearchUserAccess() {
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

                    var Search_AccessName = $("#Search_AccessName").val();
                    var Search_Admin = $("#Search_Admin").val();

                    var data = $.Deferred();

                    $.ajax({
                        type: "POST"
                        , url: Path + "/GetUser_Access"
                        , data: {
                            "AccessName": Search_AccessName,
                            "Admin": Search_Admin,
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
                        return $("<input id='CbID' value='" + item.AccessId + "'>").attr("type", "checkbox").attr("class", "CbID")
                            .prop("checked", $.inArray(item, selectedItems) > -1)
                            .on("change", function () {
                                $(this).is(":checked") ? selectItem(item) : unselectItem(item);
                            });
                    },
                    align: "center",
                    width: 50
                },
                { name: "AccessName", title: "Access Name", type: "text", width: 120 },              
                { name: "Admin", type: "select", items: [{ Name: "Admin Only", Id: "1" }, { Name: "All User", Id: "0" }], valueField: "Id", textField: "Name", width: 80 },
                {                    
                    itemTemplate: function (_, item) {
                        return $("<button id='btnConfigID" + item.AccessId + "' value='" + item.AccessId + "' data-toggle='modal' data-target='#modal-lg-Object'  onclick=\"Get_Data_Access_Object('" + item.AccessId + "')\">").attr("type", "button").text("Config Menu")
                            .on("click", function () {
                                /////// START ////////
                            });
                    }, width: 80, align: "center"
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
                        return $("<button id='btnID" + item.AccessId + "' value='" + item.AccessId + "'>").attr("type", "button").text("Edit")
                            .on("click", function () {

                                document.getElementById("HiddenMode").value = "E";
                                document.getElementById("HiddenID").value = item.AccessId;

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
                DeleteData(deletingClients[i].AccessId);
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

            $("#Access_Name").val(client.AccessName);

            $("#Admin_mode").val(client.Admin).trigger('change');

            if (dialogType == "Add") {
                $("#Admin_mode").prop("selectedIndex", 0).trigger('change');
            }

            $("#detailsDialog").dialog("option", "title", dialogType + " Client")
                .dialog("open");
        };

        function DialogSaveData() {

            if ($("#Access_Name").val().trim() != "") {

                var Mode_Ins_Edt = document.getElementById("HiddenMode").value;

                var Id_Ins_Edt = document.getElementById("HiddenID").value;

                if (Mode_Ins_Edt == "I") {

                    User_Access_Save("I", "", $("#Access_Name").val().trim(), $("#Admin_mode").val().trim());
                }
                else {

                    User_Access_Save("E", Id_Ins_Edt, $("#Access_Name").val().trim(), $("#Admin_mode").val().trim());
                }

            }
            else {
                Alert("Please input Access Name !");
            }
        }

        function DeleteData(Id_) {

            if (Id_ != "") {
                User_Access_Save("D", Id_, "", "");
            }
            else {
                Alert("Please select delete data !");
            }

        }

        function User_Access_Save(Mode_, Id_, Access_Name, Admin_mode) {

            var Sql_ = "";
          
            if (Mode_ == "I") {
                Sql_ = " INSERT INTO [dbo].[Data_User_Access]([AccessName],[Admin])"
                Sql_ += " VALUES('" + Access_Name + "','" + Admin_mode + "')";
            }
            else if (Mode_ == "E") {
                Sql_ = " UPDATE [dbo].[Data_User_Access] SET AccessName = '" + Access_Name + "',Admin = '" + Admin_mode + "' WHERE [AccessId] =" + Id_;
            }
            else {
                Sql_ = " DELETE FROM [dbo].[Data_User_Access] WHERE [AccessId] =" + Id_;
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
                        else {
                            DeleteObjectData(Id_,"A");
                        }
                    }, 1600);
                }
                , timeout: 6000
            });
        }

        $('#ObjectMenu').select2({
            placeholder: "Please select",
            allowClear: true // This is for clear get the clear button if wanted
        });

        function Get_Data_Access_Object(Access_) {

            GetObject("ObjectMenu", Access_);

            ResetTable("TabConfig_Access");
                        
            document.getElementById("AccessID").value = Access_;

            $.ajax({
                method: "POST"
                , url: Path + "/Config_Access_Object"
                , data: { "Access": Access_ }
                , dataType: "json"
                , cache: false
                , async: true

                , success: function (response) {
                    let data = eval(response);

                    let i = 0;

                    for (let i = 0; i < data.length; i++) {

                        let v = data[i];

                        Edit_Data_Access_Object(v.Id, v.AccessId, v.ObjectId, v.Obj_Desc);

                        continue;
                    }
                }
                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                    Alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                }
                , complete: function (XMLHttpRequest, textStatus) {
                    window.setTimeout(function () {
                        //// 
                    }, 1600);
                }
                , timeout: 6000
            });
        }

        function Edit_Data_Access_Object(Id_, Access_, ObjectId_, Obj_Desc_) {

            if (Id_ != "") {

                let tb = document.getElementById("TabConfig_Access");
                let tbody = tb.querySelector("tbody");

                let cols = ""
                    + "<td  style=\"vertical-align: middle\; \"><label class='FontLabel' id='lbl" + Id_ + "'>" + Obj_Desc_ + "</label></td>"
                    + "<td style=\"vertical-align: middle\; text-align:center\;\" class=\"tablecolheight\" >"
                    + "<button type =\"button\" class=\"btn btn-default\" onclick=\"DeleteObjectData('" + Id_ + "','D')\">"
                    + "<i class=\"fas fa-trash\" ></i > "
                    + " </button > "
                    + "</td > ";

                let tr = document.createElement("tr");
                tr.innerHTML = cols;

                tbody.appendChild(tr);
            }
            else {
                Alert('Data not found !');
            }

        }

        function Add_Access() {

            User_Access_Object_Save("I", "", document.getElementById("AccessID").value, $("#ObjectMenu").val());
        }

        function DeleteObjectData(Id_,mode) {

            if (Id_ != "") {
                User_Access_Object_Save(mode, Id_, document.getElementById("AccessID").value,"");
            }
            else {
                Alert("Please select delete data !");
            }
        }

        function User_Access_Object_Save(Mode_, Id_, AccessId, ObjectId) {

            var Sql_ = "";

            if (Mode_ == "I") {
                Sql_ = " INSERT INTO [dbo].[Config_Access_Object]([AccessId],[ObjectId])"
                Sql_ += " VALUES('" + AccessId + "','" + ObjectId + "')";
            }
            else if (Mode_ == "A")
            {
                Sql_ = " DELETE FROM [dbo].[Config_Access_Object] WHERE [AccessId] =" + Id_;
            }
            else {
                Sql_ = " DELETE FROM [dbo].[Config_Access_Object] WHERE [Id] =" + Id_;
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
                        if (Mode_ != "A") {
                            Completed("Update data completed ! ");
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
                        if (Mode_ != "A") {
                            Get_Data_Access_Object(AccessId);
                            $("#modal-lg-Object").modal('show');
                        }
                    }, 1600);
                }
                , timeout: 6000
            });
        }

    </script>
</asp:Content>
