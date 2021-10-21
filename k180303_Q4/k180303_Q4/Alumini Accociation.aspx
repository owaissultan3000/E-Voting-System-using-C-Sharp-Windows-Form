<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Alumini Accociation.aspx.cs" Inherits="k180303_Q4.Alumini_Accociation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Alumini Accociation Results</title>
    <style>
        table {
          font-family: arial, sans-serif;
          border-collapse: collapse;
          width: 100%;
          margin-top: 50px;
        }
        
        td{
          border: 1px solid #dddddd;
          text-align: left;
          padding: 8px;
          font-weight: bold;
           font-size: 20px;
            background-color: rgb(122, 179, 226);
        }
        th {
          border: 1px solid #dddddd;
          text-align: left;
          padding: 8px;
        }
        
        tr{
            background-color: rgb(135, 166, 190); 
            font-family:'Georgia', 'Times New Roman';
        }
        
        </style>
    
</head>
<body>
    <form id="form1" runat="server">
       <div style="display: flex; flex-direction: column; justify-content: center; width: 100%; height: 98vh; background-color: #ADD8E6; background-attachment: fixed;">
        <div style="display: flex; width: 100%; align-items: center; margin-top: -15%;">
            <div style="margin-left: 13%;">
                <img src="./Image/NU_logo.png"style="width:250px; height:250px;" />
            </div>
            <div style="margin-left: 30px;">
                <h1 style="font-size:4rem">"Alumini Association Election-2021 Results"</h1>
            </div>
        </div>
        
        <div style=" margin-top: 70px; display: flex; justify-content:center; width: 80%; margin-left: 10%; " >
            
            <div style="width: 35%;">
            <table id="table1">
                <thead>
                    <tr>
                        <td colspan = "4" style="text-align: center; font-size: 28px;">"President"</td>
                    </tr>

                    <tr>
                        <th>Candidate Name</th>
                        <th>Votes</th>
                    </tr>
                </thead>
                <tbody>
                    <% foreach (KeyValuePair<string, int> pair in President)
                                { %>
                            <tr>
                                <td><%= pair.Key %> </td>
                                <td><%=pair.Value%></td>
                            </tr>
                            <% } %>   
                </tbody>
                
              </table>
            </div>

              <div style="width: 35%; margin-left: 2%;">
              <table id="table2">
                <thead>
                    <tr>
                        <td colspan = "4" style="text-align: center; font-size: 28px;">"Vice President"</td>
                     </tr>
                    <tr>
                        <th>Candidate Name</th>
                        <th>Votes</th>
                    </tr>
                </thead>
                <tbody>
                     <% foreach (KeyValuePair<string, int> pair in VicePresident)
                                { %>
                            <tr>
                                <td><%= pair.Key %> </td>
                                <td><%=pair.Value%></td>
                            </tr>
                            <% } %>   
                </tbody>
               
              </table>
            </div>

            <div style="width: 35%; margin-left: 2%;">
              <table id="table3">
                <thead>
                    <tr>
                        <td colspan = "4" style="text-align: center; font-size: 28px;">"General Secretary"</td>
                     </tr>
                    <tr>
                        <th>Candidate Name</th>
                        <th>Votes</th>
                    </tr>
                </thead>
                <tbody>
                           <% foreach (KeyValuePair<string, int> pair in GeneralSecratery)
                                { %>
                            <tr>
                                <td><%= pair.Key %> </td>
                                <td><%=pair.Value%></td>
                            </tr>
                            <% } %>         
                </tbody>
                
              </table>
            </div>
        
        </div>

    </div>

    </form>

</body>
</html>




<script type="text/javascript">
    setTimeout(function () { location.reload(1); }, 300000 );
</script>
