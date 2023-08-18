var skipNextContextMenu = false;

$(function() {

    // Trigger action when the contexmenu is about to be shown
    
    $(document).bind("contextmenu", function (event) {
        if (!event.altKey && !event.ctrlKey && !event.shiftKey && event.button === 2) {

            // Avoid the real one
            //event.stopImmediatePropagation();
            event.preventDefault();

            if(skipNextContextMenu) {
                skipNextContextMenu = false;
                return;
            }

            if($('.modal.in, .modal.show').length) {
                return false;
            }

            //TODO: Detect target and set menu item d-none / callback
            // if not over item which supports a menu, return false;

            if($('.custom-menu').css('display') === 'none') {
                $(event.target).click();
                // Show contextmenu
                $(".custom-menu").finish().toggle(0)
                    .css({
                        top: event.pageY + "px",
                        left: event.pageX + "px"
                });
            }
        }
    });
    

    // If the document is clicked somewhere
    $(document).bind("mousedown", function (e) {
        //2 == right button
        if(e.button === 2 && $(e.target).parents(".custom-menu").length > 0) {
            skipNextContextMenu = true;
            e.preventDefault();
        } else {
            // If the clicked element is not the menu
            if (!$(e.target).parents(".custom-menu").length > 0) {
                menuHide();
            }
        }
    });

    $(document).bind("mouseup", function (e) {
        //2 == right button
        if(e.button === 2 && $(e.target).parents(".custom-menu").length > 0) {
            e.preventDefault();
            $(e.target).click();
        }
    });    

    // If the menu element is clicked
    $(document).on('click', ".custom-menu li", function(){
        
        var callback = $(this).data("callback");
        if (callback) {
            var fn = window[callback];
            if (typeof fn === 'function') {
                fn($(this).data('menudata'));
            }
        }        
        // Hide it AFTER the action was triggered
        menuHide();
    });
});

function menutest(msg) {
    alert(msg);
}

function menuHide() {
    $(".custom-menu").hide(0);
}