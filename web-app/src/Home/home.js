$(document).ready(function(){
  $('.addBtn').on('click', function(){
    addTableRow();
  });
  $('.addBtnRemove').click(function(){
    $(this).closest('tr').remove();
  })

  var i = 1;
  function addTableRow(){
    var tempTr = $('<tr><td><input type="text" id="userName_' + i + 
    '" class="form-control"/></td><td><input type="text" id="email_' + i + 
    '" class="form-control" /></td><td><input type="text" id="password_' + i + 
    '" class="form-control" /></td><td><span class="glyphicon glyphicon-minus addBtnRemove" id="addBtn_' + i + 
    '"></span></td></tr>').on('click', function () {
      $(this).closest('tr').remove(); 
      $(document.body).on('click', '.TreatmentHistoryRemove', function (e) {
           $(this).closest('tr').remove();  
       });
   });
   $("#tableAddRow").append(tempTr)
   i++;
}
});
