class PrintVehiclelabel {
    constructor(pageIndex) {
        this.pageIndex = pageIndex;
        if (!jQuery) { throw new Error("Bootstrap requires jQuery") }
    }
    
    createPage(data_) {
        let header = this.createBody(data_);

        let pageAlignment = document.createElement("div");
        pageAlignment.classList.add("page-inner");
        pageAlignment.appendChild(header);
        
        let page = document.createElement("div");
        page.classList.add("page");
        page.classList.add("pageIdex"+ this.pageIndex);
        page.appendChild(pageAlignment);

        return page;
    }

    createBody(data) {
        let tb = document.createElement("table");
        tb.setAttribute("id", "body");
        tb.style.width = '98%';
	 tb.style.marginTop = '2px';
        tb.style.marginLeft = '15px';
        tb.style.tableLayout = 'fixed';

        
        let str_builder = '<tbody>';

        let str_sub_builder = '';

        var count = 1 ;

        for (let i = 0; i < data.length; i++) {

            let dt = data[i];  

            if (count == 1 )
            {
                str_builder += '<tr width="100%">'
            }

            str_sub_builder = '<table height="99%" width="265px"><tbody height="100%" width="265px">'
                +'<TR height="45px"><TH style="vertical-align: top; text-align: left;" colspan="2"><img src="Images/logo/K_Express_home.png" style="height: 60px; width: 165px" /> ' ;

            if (dt.Driver_Image_chk=='1')
            {
                str_sub_builder += '<TR height="125px"><TH style="vertical-align: top; text-align: center;" height="125px"><img src="../HandlerImg.ashx?Id=' + dt.Driver_ID+'" style="width: 95px" class="img-responsive" /></TH><TH class="place-qrcode" style="vertical-align: top; text-align: center;" ><div id="qrcode'+ i +'" class="qrcode"></div></TH>';
            }
            else
            {
                str_sub_builder += '<TR height="125px"><TH style="vertical-align: top; text-align: center;" height="125px"><i class="fas fa-user-circle fa-7x" style="color: #009da5;height:124px"></TH><TH class="place-qrcode" style="vertical-align: top; text-align: center;" ><div id="qrcode'+ i +'" class="qrcode"></div></TH>';             
            }

            var supplier_name = "";

            supplier_name = dt.Vehicle_Supplier_Name;

            const supplier_name_words = supplier_name.split(' ');
            supplier_name = supplier_name_words[0];
            if (supplier_name_words.length > 1) {
                supplier_name += ' ' + supplier_name_words[1];                
            }

            str_sub_builder += '<TR height="45px"><TH style="text-align: left;font-size: 16px;background-color: #009da5;color: white;" colspan="2">&nbsp;  <i class="fas fa-user fa-lg">  </i> &nbsp; '+dt.Driver_Name +''        
                +'<TR height="45px"><TH style="text-align: left;font-size: 16px;background-color: #009da5;color: white;" colspan="2">&nbsp;  <i class="fas fa-folder fa-lg">  </i>&nbsp;  '+dt.Vehicle_Registration_No +''
                +'<TR height="45px"><TH style="text-align: left;font-size: 16px;background-color: #009da5;color: white;" colspan="2">&nbsp;  <i class="fas fa-phone-volume fa-lg"></i>&nbsp;  '+dt.Tel_Number +''
                + '<TR height="45px"><TH style="text-align: left;font-size: 16px;background-color: #009da5;color: white;" colspan="2">&nbsp;  <i class="fas fa-users fa-lg"></i>&nbsp; ' + supplier_name +''
                +'</tbody></table>';  

            str_builder += '<td class="text-center" width="100%">'+ str_sub_builder +' </td>';
            
            if (count == 3 )
            {                
                str_builder += '</tr>' ;
                count = 1 ;
                str_builder +='<tr width="100%" height="10px"></tr>'
            }
            else
            {
                count++;
            }
        }

        str_builder += '<tbody>';

        tb.innerHTML = '<thead></thead>'+str_builder ;

        return tb;
    }
}
