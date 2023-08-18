$(function() {

    $('#modalStandardCenter').on('show.bs.modal', function(e) { 
        let buttons = $(e.relatedTarget).data('buttons');
        if (modalStandardCenterButtons || false) {
            buttons = modalStandardCenterButtons;
        }        
        if(buttons) {
            let btn = buttons.split(",");
            for(let i = 0; i < btn.length; i++) {
                let b = btn[i];
                let cb = '';
                if(b.indexOf('|' > -1)) {
                    let vals = b.split('|');
                    b = vals[0];
                    cb = vals[1];
                }
                $('#modalStandardCenter .modalbtn-' + b).removeClass('d-none');
                $('#modalStandardCenter .modalbtn-' + b).data('callback', cb);
            }
        } else {
            $('#modalStandardCenter .modalbtn-close').removeClass('d-none');
        }
        let title = $(e.relatedTarget).data('title');
        if (modalStandardCenterTitle || false) {
            title = modalStandardCenterTitle;
        }
        if(title.startsWith('#')) {
            title = $(title).html();
        }
        $('#modalStandardCenter .modal-title').html(title);

        let body = $(e.relatedTarget).data('body');
        if (modalStandardCenterBody || false) {
            body = modalStandardCenterBody;
        }        
        if(body && body.startsWith('#')) {
            body = $(body).html();
        }
        $('#modalStandardCenter .modal-body').html(body);        

        var showcallback = $(e.relatedTarget).data("showcallback");
        if(!showcallback) {
            showcallback = modalStandardShowCallback;
        }
        if (showcallback) {
            var fn = window[showcallback];
            if (typeof fn === 'function') {
                fn();
            } else {
                eval(showcallback);
            }
        }

    });

    $('#modalStandardCenter').on('hidden.bs.modal', function (e) {
        $('#modalStandardCenter .modal-footer button').addClass('d-none');
        $('#modalStandardCenter .modal-footer button').data('callback','');
        $('#modalStandardCenter').find('.modal-dialog').removeClass('modal-dialog-xxl');
        modalStandardShowCallback = null;
        modalStandardCenterBody = null;
        modalStandardCenterButtons = null;
        modalStandardCenterTitle = null;
    })

    $('#modalStandardCenter .btn').click(function(e) {
        var callback = $(this).data("callback");
        if (callback) {
            var fn = window[callback];
            if (typeof fn === 'function') {
                fn();
            } else {
                eval(callback);
            }
        }
    });

});

function modalDismissStandardCenter() {
    $('#modalStandardCenter').modal('hide');
}
