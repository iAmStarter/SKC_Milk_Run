var dateObj = new Date();
var month = dateObj.getUTCMonth() + 1;
var day = dateObj.getUTCDate();
var year = dateObj.getUTCFullYear();

function Alert(msg) {

    toastr.error('<label style="font-size:14px;">' + msg + '</label>');
}

function Completed(msg) {

    toastr.success('<label style="font-size:14px;">' + msg + '</label>');
}
function changeTime(k) { /* appending 0 before time elements if less than 10 */
    if (k < 10) {
        return "0" + k;
    }
    else {
        return k;
    }
}

function Notecommit(ID_, LICENCE_, Note_) {

    $.ajax({
        method: "POST"
        , url: Path + "/InsertUpdateNote"
        , data: {
            "ID": ID_,
            "Note": Note_
        }
        , dataType: "json"
        , cache: false
        , beforeSend: function () {
        }
        , success: function (response) {
            let data = eval(response);

            if (data['0'].Status == "Y") {
                checkStatus = 1;
            }
            else {
                Alert(data['0'].Message);
                checkStatus = 0;
            }
        }
        , error: function (XMLHttpRequest, textStatus, errorThrown) {
            checkStatus = 0;
            Alert("Error : " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        }
        , complete: function (XMLHttpRequest, textStatus) {
            window.setTimeout(function () {
                if (checkStatus == 1) {

                    Completed("Update Note completed !");
                    Show_Monitoring_Data();
                }
                else {
                    Alert('Update Note not completed !');
                }
            }, 100);
        }
        , timeout: 0

    });

}

function getCurrentOilPrice(name_obj, YYYY_, MM_) {
    $.ajax({
        method: "POST"
        , url: Path + "/getCurrentOilPrice"
        , data: { "YYYY": YYYY_, "MM": MM_ }
        , dataType: "json"
        , cache: false
        , async: true
        , success: function (response) {
            let data = eval(response);

            let i = 0;
            let currentScan = true;
            if (data.length > 0) {
                let v = data[i];

                document.getElementById(name_obj).value = parseFloat(v.Diesel_B7).toFixed(2);
            }
        }
        , error: function (XMLHttpRequest, textStatus, errorThrown) {
            Alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        }
        , complete: function (XMLHttpRequest, textStatus) { }
        , timeout: 0
    });
}

function selectMultiple_NOT_IN(selectinput_) {

    var selectinput_var_ = $('#' + selectinput_).select2("data");

    var where_ = "All";

    if (selectinput_var_.length > 0 ) {
        where_ = "";
    }

    for (var i = 0; i <= selectinput_var_.length - 1; i++) {

        if (selectinput_var_[i].id.trim() != "") {

            if (where_ == "") {
                where_ = selectinput_var_[i].id.trim() ;
            }
            else {
                where_ += " ," + selectinput_var_[i].id.trim() ;
            }
        }
    }

    return where_;
}

function selectMultiple(selectinput_) {
    let run = 0;
    var selectinput_var_ = $('#' + selectinput_).select2("data");

    var where_ = " LIKE '%' ";

    for (var i = 0; i <= selectinput_var_.length - 1; i++) {

        if (selectinput_var_[i].id.trim() != "") {

            if (run == 0) {
                where_ = " IN ( ";
            }
            if (selectinput_var_[i].id.trim() != "") {
                if (run == 0) {
                    where_ += "'" + selectinput_var_[i].id.trim() + "'";
                }
                else {
                    where_ += " ,'" + selectinput_var_[i].id.trim() + "'";
                }
            }

            if (i == (selectinput_var_.length - 1)) {
                where_ += " ) ";
            }

            run++;
        }
    }

    return where_;
}

function Supplier_sql_In(UserLevel_, SupplierCode, mode_ = "") {

    var SQLCommand = "", supplier_ = "";

    var UserLevel = "", where_ = "";

    if (UserLevel_.trim() != "")
        UserLevel = UserLevel_.trim();

    if (SupplierCode.trim() != "") {
        supplier_ = SupplierCode.trim();
        where_ = "WHERE Supplier_Code ='" + supplier_ + "'";
    }
    else
        where_ = "WHERE Supplier_Code = '' ";

    if (supplier_ == "SKC") {
        supplier_ = "";
        where_ = "";
    }
    else {
        if (mode_ == "M") {
            if (UserLevel.trim() == "2") {
                supplier_ = "";
                where_ = "";
            }
        }
    }

    if (UserLevel.trim() == "3") {
        supplier_ = "";
        where_ = "";
    }

    SQLCommand = "SELECT Supplier_Code COLLATE DATABASE_DEFAULT FROM [Tbl_Supplier] " + where_;

    return SQLCommand;
}

function Supplier_sql(UserLevel_, SupplierCode, user_name, mode_ = "") {

    var SQLCommand = "", supplier_ = "";

    var UserLevel = "";

    if (UserLevel_.trim() != "")
        UserLevel = UserLevel_.trim();

    if (SupplierCode.trim() != "")
        supplier_ = SupplierCode.trim();


    if (supplier_ == "SKC") {
        supplier_ = "";
    }
    else {
        if (mode_ == "M") {
            if (UserLevel.trim() == "2") {
                supplier_ = "";
            }
        }
    }

    if (UserLevel.trim() == "3") {
        supplier_ = "";
    }

    SQLCommand = "SELECT Supplier_Code COLLATE DATABASE_DEFAULT  FROM [Tbl_Supplier] WHERE Supplier_Code LIKE '%" + supplier_.trim() + "%'";

    return SQLCommand;
}



function Numbervalidate(evt) {
    var theEvent = evt || window.event;

    // Handle paste
    if (theEvent.type === 'paste') {
        key = event.clipboardData.getData('text/plain');
    } else {
        // Handle key press
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
    }
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}


function Cal_Date(mode_, Time_, Date_, Shift_) {
    // Mode : 0 = Sysdate , 1 : Date value
    var date = new Date();

    var str_time = Time_.replace(':', '.');
    var number_time = parseFloat(str_time);

    var Timesplit = [];

    var Daysplit = [];

    if (Time_ != "") {
        Timesplit = Time_.split(':');
    }

    if (Date_ != "") {
        Daysplit = Date_.split('/');
    }

    if (Shift_ == "NIGHT") {
        date.setDate(date.getDate() + 1);
    }
    else if (Shift_ == "END") {
        if (number_time < 8) {
            date.setDate(date.getDate() + 1);
        }
    }

    var dd = String(date.getDate()).padStart(2, '0');
    var mm = String(date.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = date.getFullYear();

    if (mode_ == "0") {
        return new Date(yyyy, mm, dd, Timesplit[0], Timesplit[1]);
    }
    else {
        return new Date(Daysplit[2], Daysplit[1], Daysplit[0], Timesplit[0], Timesplit[1]);
    }
}

function setFontColor(hex_color) {
    var hex_ = true;
    var textColour = 'black';

    if (!hex_color || typeof hex_color !== 'string') {

        textColour = 'black';
    }
    else {
        var color = hex_color;
        if (color.substring(0, 1) === '#') {

            color = color.substring(1);

            if (color.length == 3) {
                hex_ = /^[0-9A-F]{3}$/i.test(color);
            }
            else if (color.length == 6) {
                hex_ = /^[0-9A-F]{6}$/i.test(color);
            }
            else if (color.length == 8) {
                hex_ = /^[0-9A-F]{8}$/i.test(color);
            }
            else {
                hex_ = false;
            }

            if (hex_ == true) {
                const rgb = [hexToRgb(hex_color).r, hexToRgb(hex_color).g, hexToRgb(hex_color).b];

                const brightness = Math.round(((parseInt(rgb[0]) * 299) +
                    (parseInt(rgb[1]) * 587) +
                    (parseInt(rgb[2]) * 114)) / 1000);

                textColour = (brightness > 125) ? 'black' : 'white';

            }
            else {
                textColour = 'black';
            }
        }
        else {
            textColour = 'black';
        }
    }

    return textColour;
}

function hexToRgb(hex) {

    if (hex == null || hex == '') {
        hex = "#00bfff";
    }

    if (hex == "")
        hex = "#00bfff";

    var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return result ? {
        r: parseInt(result[1], 16),
        g: parseInt(result[2], 16),
        b: parseInt(result[3], 16)
    } : null;
}

function ResetTable(TableName) {
    let tb = document.getElementById(TableName);
    let tbody = tb.querySelector("tbody");

    while (tbody.childNodes.length) {
        tbody.removeChild(tbody.firstChild);
    }
}

function deleteMoreRows(tableID) {

    var table = document.getElementById(tableID);
    var rowCount = table.rows.length;
    var selectedRows = getCheckedBoxes();

    selectedRows.forEach(function (currentValue) {
        if (currentValue.id != "GateShift_D_N" && currentValue.id != "S_Shift_D_N") {
            deleteRowByCheckboxId(currentValue.id);
        }
    });

}

function deleteRowByCheckboxId(CheckboxId) {

    var checkbox = document.getElementById(CheckboxId);
    var row = checkbox.parentNode.parentNode; //box, cell, row, table
    var table = row.parentNode;

    while (table && table.tagName != 'TABLE')
        table = table.parentNode;
    if (!table) return;
    table.deleteRow(row.rowIndex);
}

function getCheckedBoxes() {
    var inputs = document.getElementsByTagName("input");
    var checkboxesChecked = [];

    // loop over them all
    for (var i = 0; i < inputs.length; i++) {
        // And stick the checked ones onto an array...
        if (inputs[i].checked) {
            checkboxesChecked.push(inputs[i]);
        }
    }

    // Return the array if it is non-empty, or null
    return checkboxesChecked.length > 0 ? checkboxesChecked : null;
}

function GetSuppliersselect(select_, ID_) {

    let select_box = document.getElementById(select_);
    var options = "";
    removeOptions_select2(select_);

    options = '<option value="">Select</option>';
    select_box.innerHTML = select_box.innerHTML + options;

    $.ajax({
        method: "POST"
        , url: Path + "/Suppliers_list_Vehicle"
        , data: {
            "ID": ID_
        }
        , dataType: "json"
        , cache: false
        , async: true

        , success: function (response) {
            let data = eval(response);

            let i = 0;

            for (let i = 0; i < data.length; i++) {
                let v = data[i];

                options = '<option value="' + v.Supplier_Code + '">' + v.Supplier_Name + '</option>';

                select_box.innerHTML += options;
                continue;

            }
        }
        , error: function (XMLHttpRequest, textStatus, errorThrown) {
            Alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        }
        , timeout: 0
    });
}

function GetObject(select_, Access) {

    let select_box = document.getElementById(select_);
    var options = "";
    removeOptions_select2(select_);

    options = '<option value="">Select</option>';
    select_box.innerHTML = select_box.innerHTML + options;

    $.ajax({
        method: "POST"
        , url: Path + "/Data_Object"
        , data: {
            "Access": Access
        }
        , dataType: "json"
        , cache: false
        , async: true

        , success: function (response) {
            let data = eval(response);

            let i = 0;

            for (let i = 0; i < data.length; i++) {
                let v = data[i];

                options = '<option value="' + v.ObjectId + '">' + v.Description + '</option>';

                select_box.innerHTML += options;
                continue;

            }
        }
        , error: function (XMLHttpRequest, textStatus, errorThrown) {
            Alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        }
        , timeout: 0
    });
}

function GetUser_Access(select_, UserLevel, Supplier, mode) {

    let select_box = document.getElementById(select_);
    var options = "";
    removeOptions_select2(select_);

    if (mode == "S") {
        options = '<option value="">Select</option>';
        select_box.innerHTML = select_box.innerHTML + options;
    }

    $.ajax({
        method: "POST"
        , url: Path + "/User_Access"
        , data: {
            "UserLevel": UserLevel, "Supplier": Supplier
        }
        , dataType: "json"
        , cache: false
        , async: true

        , success: function (response) {
            let data = eval(response);

            let i = 0;

            for (let i = 0; i < data.length; i++) {
                let v = data[i];

                options = '<option value="' + v.AccessId + '">' + v.AccessName + '</option>';

                select_box.innerHTML += options;
                continue;

            }
        }
        , error: function (XMLHttpRequest, textStatus, errorThrown) {
            Alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        }
        , timeout: 0
    });
}

function GetUser_Level(select_, UserLevel, mode) {

    let select_box = document.getElementById(select_);
    var options = "";
    removeOptions_select2(select_);

    if (mode == "S") {
        options = '<option value="">Select</option>';
        select_box.innerHTML = select_box.innerHTML + options;
    }

    $.ajax({
        method: "POST"
        , url: Path + "/User_Level"
        , data: { "UserLevel": UserLevel }
        , dataType: "json"
        , cache: false
        , async: true

        , success: function (response) {
            let data = eval(response);

            let i = 0;

            for (let i = 0; i < data.length; i++) {
                let v = data[i];

                options = '<option value="' + v.LevelId + '">' + v.LevelName + '</option>';

                select_box.innerHTML += options;
                continue;

            }
        }
        , error: function (XMLHttpRequest, textStatus, errorThrown) {
            Alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        }
        , timeout: 0
    });
}

function GetSupplierlist(select_, mode, UserLevel_, SupplierCode, user_name) {

    let select_box = document.getElementById(select_);
    var options = "";
    removeOptions_select2(select_);

    options = '<option value="">Select</option>';
    select_box.innerHTML = select_box.innerHTML + options;

    $.ajax({
        method: "POST"
        , url: Path + "/Supplier_list"
        , data: { "UserLevel_": UserLevel_, "SupplierCode": SupplierCode, "user_name": user_name }
        , dataType: "json"
        , cache: false
        , async: true

        , success: function (response) {
            let data = eval(response);

            let i = 0;

            for (let i = 0; i < data.length; i++) {
                let v = data[i];
                if (mode == "S") {
                    options = '<option value="' + v.Supplier_No + '">' + v.Short_Name + '</option>';
                }
                else {
                    options = '<option value="' + v.Supplier_No + '">(' + v.Short_Name + ') ' + v.Supplier_Name + '</option>';
                }
                select_box.innerHTML += options;
                continue;

            }

        }
        , error: function (XMLHttpRequest, textStatus, errorThrown) {
            Alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        }
        , timeout: 0
    });
}

function selectItemByValue(elmnt_, value) {
    var elmnt = document.getElementById(elmnt_);
    for (var i = 0; i < elmnt.options.length; i++) {
        if (elmnt.options[i].value == value) {
            elmnt.getElementsByTagName('option')[i].selected = true;
            break;
        }
    }
}

function GetDrivers(selectDrivers_, mode, Supplier_Code_) {

    let selectDrivers = document.getElementById(selectDrivers_);
    var options = "";

    removeOptions_select2(selectDrivers_);

    $.ajax({
        method: "POST"
        , url: Path + "/GetDrivers"
        , data: {
            "TRANSPORTER_": Supplier_Code_,
            "ID_": ""
        }
        , dataType: "json"
        , cache: false
        , async: true

        , success: function (response) {
            let data = eval(response);

            let i = 0;

            if (mode == "A") {
                options = '<option value="">All</option>';
            }
            else {
                options = '<option value="">Select</option>';
            }
            selectDrivers.innerHTML = options;

            for (let i = 0; i < data.length; i++) {
                let v = data[i];
                if (mode == "S") {
                    options = '<option value="' + v.ID + '">' + v.Name + '</option>';
                }
                else {
                    options = '<option value="' + v.ID + '">' + v.Name + '(' + v.Supplier_Code + ')</option>';
                }
                selectDrivers.innerHTML = selectDrivers.innerHTML + options;
                continue;
            }

        }
        , error: function (XMLHttpRequest, textStatus, errorThrown) {
            Alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        }, complete: function (XMLHttpRequest, textStatus) {

        }
        , timeout: 0
    });
}
function GetSupplierWhereType(selectTRANSPORTER, mode, UserLevel, User_supplier_code, user_name, Type_) {

    let select_TRANSPORTER = document.getElementById(selectTRANSPORTER);
    var options = "";
    removeOptions_select2(selectTRANSPORTER);

    $.ajax({
        method: "POST"
        , url: Path + "/GetSupplierTransport"
        , data: { "supplier_code": User_supplier_code, "Type": Type_, "userlevel_": UserLevel, "user_name_": user_name }
        , dataType: "json"
        , cache: false
        , async: true

        , success: function (response) {

            let data = eval(response);

            let i = 0;

            if (mode == "A") {
                options = '<option value="">All</option>';
            }
            else if (mode == "SA") {
                options = '<option value="">All</option>';
            }
            else {
                options = '<option value="">Select</option>';
            }
            select_TRANSPORTER.innerHTML = options;

            for (let i = 0; i < data.length; i++) {
                let v = data[i];
                if (mode == "S") {
                    options = '<option value="' + v.Supplier_Code + '">' + v.Short_Name + '</option>';
                }
                else if (mode == "SA") {
                    options = '<option value="' + v.Supplier_Code + '">' + v.Short_Name + '</option>';
                }
                else if (mode == "N") {
                    options = '<option value="' + v.Supplier_Code + '">' + v.Supplier_Name + '</option>';
                }
                else {
                    options = '<option value="' + v.Supplier_Code + '">(' + v.Short_Name + ') ' + v.Supplier_Name + '</option>';
                }
                select_TRANSPORTER.innerHTML = select_TRANSPORTER.innerHTML + options;
                continue;

            }

            $("#" + selectTRANSPORTER).val("").trigger('change');

        }
        , error: function (XMLHttpRequest, textStatus, errorThrown) {
            Alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        }, complete: function (XMLHttpRequest, textStatus) {
            window.setTimeout(function () {

            }, 0);
        }
        , timeout: 0
    });
}

function GetSupplierTransport(selectTRANSPORTER, mode, UserLevel, User_supplier_code, user_name) {

    let select_TRANSPORTER = document.getElementById(selectTRANSPORTER);
    var options = "";
    removeOptions_select2(selectTRANSPORTER);

    $.ajax({
        method: "POST"
        , url: Path + "/GetSupplierTransport"
        , data: { "supplier_code": User_supplier_code, "Type": "", "userlevel_": UserLevel, "user_name_": user_name }
        , dataType: "json"
        , cache: false
        , async: true

        , success: function (response) {
            let data = eval(response);

            let i = 0;

            if (mode == "A") {
                options = '<option value="">All</option>';
            }
            else {
                options = '<option value="">Select</option>';
            }
            select_TRANSPORTER.innerHTML = options;

            for (let i = 0; i < data.length; i++) {
                let v = data[i];
                if (mode == "S") {
                    options = '<option value="' + v.Supplier_Code + '">' + v.Short_Name + '</option>';
                }
                else if (mode == "N") {
                    options = '<option value="' + v.Supplier_Code + '">' + v.Supplier_Name + '</option>';
                }
                else {
                    options = '<option value="' + v.Supplier_Code + '">(' + v.Short_Name + ') ' + v.Supplier_Name + '</option>';
                }
                select_TRANSPORTER.innerHTML = select_TRANSPORTER.innerHTML + options;
                continue;

            }

            $("#" + selectTRANSPORTER).val("").trigger('change');

        }
        , error: function (XMLHttpRequest, textStatus, errorThrown) {
            Alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        }, complete: function (XMLHttpRequest, textStatus) {
            window.setTimeout(function () {

            }, 0);
        }
        , timeout: 0
    });
}

function GetROUTETransport(ID_, selectRoute, selectTRANSPORTER, UserLevel, User_supplier_code, user_name, page_, mode) {

    let select_TRANSPORTER = $("#" + selectTRANSPORTER).val();

    let selectRoute_ = document.getElementById(selectRoute);
    var options = "";

    removeOptions_select2(selectRoute);

    if (page_ == "LIST") {

        if (select_TRANSPORTER == "")
            select_TRANSPORTER = "%";

    }

    if (select_TRANSPORTER != "") {

        $.ajax({
            method: "POST"
            , url: Path + "/GetROUTETransport"
            , data: {
                "supplier_code": select_TRANSPORTER, "user_supplier_code": User_supplier_code, "userlevel_": UserLevel, "user_name_": user_name
            }
            , dataType: "json"
            , cache: false
            , async: true

            , success: function (response) {
                let data = eval(response);

                let i = 0;
                if (mode == "A") {
                    options = '<option value="">All</option>';
                }
                else {
                    options = '<option value="">Select</option>';
                }

                selectRoute_.innerHTML = options;

                for (let i = 0; i < data.length; i++) {
                    let v = data[i];

                    options = '<option value="' + v.Id + '">' + v.Route_Code + '</option>';
                    selectRoute_.innerHTML = selectRoute_.innerHTML + options;
                    continue;

                }

                if (ID_ != "") {
                    $("#" + selectRoute).val(ID_).trigger('change');
                }
                else
                    $("#" + selectRoute).val("").trigger('change');

            }
            , error: function (XMLHttpRequest, textStatus, errorThrown) {
                Alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
            }
            , complete: function (XMLHttpRequest, textStatus) {
                window.setTimeout(function () {
                }, 600);
            }
            , timeout: 0
        });
    }
    else {
        options = '<option value="">Select</option>';
        selectRoute_.innerHTML = options;
        $("#" + selectRoute).val("").trigger('change');
    }
}

function removeOptions(selectElement) {

    var i, L = selectElement.options.length - 1;
    for (i = L; i >= 0; i--) {
        selectElement.remove(i);
    }
}

function removeOptions_select2(selectElement) {
    $("#" + selectElement).empty();
}

