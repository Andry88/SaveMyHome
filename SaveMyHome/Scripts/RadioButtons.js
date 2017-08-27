$(function() {
    var textarea = $('textarea');
    var changeOrDropMessage = function() {
        var checkedRadio = $('input[name="VisitorProblemStatus"]:checked');
        var value = checkedRadio.val();

        if (value == 'Culprit') {
            $('#message').css({ 'display': 'none' });
            $('input:submit').val(AllScripts.NotifyButton);
        }
        else
        {
            $('#message').css({ 'display': 'block' });
            $('input:submit').val(AllScripts.SendButton);

            if (value == 'Victim') {
                textarea.val(AllScripts.IWasFlooded);
            }
            else {
                    if(value == 'None' && checkedRadio.attr('id') == 'PotentialVictim') {
                        textarea.val(AllScripts.IWasNotFlooded);
                    }
                    else {
                        textarea.val(AllScripts.ThisIsNotMe);
                    }
            }
        }
    }
        changeOrDropMessage();

    $('.radio').change(function () {
        changeOrDropMessage();
    });
});
