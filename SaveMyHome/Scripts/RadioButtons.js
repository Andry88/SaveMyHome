
    $(function() {
            var textarea = $('textarea');
            var changeOrDropMessage = function() {
                var checkedRadio = $('input[name="VisitorProblemStatus"]:checked');
                var value = checkedRadio.val();

                if (value == 'Culprit') {
                    $('#message').css({ 'display': 'none' });
                    $('input:submit').val('Оповестить остальных жильцов о потопе');
                }
                else
                {
                    $('#message').css({ 'display': 'block' });
                    $('input:submit').val('Отправить');

                    if (value == 'Victim') {
                        textarea.val('Меня затопило');
                    }
                    else {

                          if (value == 'None' && checkedRadio.attr('id') == 'PotentialVictim') {
                            textarea.val('Меня не затопило');
                          }
                          else {
                                textarea.val('Это не я');
                          }
                    }
                }
            }
             changeOrDropMessage();

            $('.radio').change(function () {
             changeOrDropMessage();
            });
    });
