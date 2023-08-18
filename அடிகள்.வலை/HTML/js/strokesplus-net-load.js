
var modalStandardCenterTitle;
var modalStandardCenterBody;
var modalStandardCenterButtons;
var modalStandardShowCallback;

var applyClicked = false;
var lastNewGestureName = '';


$(function() {
    
    navLoadSection();
    
    $('.sidebar-sticky').find('.nav-item').click(function(){
        if ($(this).children('a.active').length == 0) {
            $('.sidebar ul li').find('.active').removeClass('active');
            $(this).children('a').addClass('active');
            navLoadSection();
        }
    });

    $(window).resize(function() {
        if(window.currentSection.ActionList && window.currentSection.ActionList.ActionEditor && window.currentSection.ActionList.ActionEditor.StepsScriptEditor.ScriptEditor.CodeMirror) {
            window.currentSection.ActionList.ActionEditor.StepsScriptEditor.ScriptEditor.CodeMirror.setSize(100, 100);
        }        
        settingsResizeElements();
    });    

    $('#buttonSettingsApply').click(function (b) {
        settingsMainFormButtonsDisable();
        settingsUpdateAppSettings();
        log('#buttonSettingsApply.click()');
        navSaveAllVisibleSections();
        hostPostMessage('Apply', '', appSettings);
    });

    $('#buttonSettingsOK').click(function (b) {
        settingsMainFormButtonsDisable();
        settingsUpdateAppSettings();
        log('#buttonSettingsOK.click()');
        navSaveAllVisibleSections();
        hostPostMessage('OK', '', appSettings);
    });

    $('#buttonSettingsCancel').click(function (b) {
        settingsMainFormButtonsDisable();
        log('#buttonSettingsCancel.click()');
        hostPostMessage('Cancel', '', appSettings);
    });

    $('#ToggleSidebar').click(function (b) {
        navToggleSidebar();
    });

    $(document).on('click', '.form-check', function (b) {
        if(b.target.type !== "checkbox" && b.target.nodeName !== "LABEL") {
            $(this).children('.form-check-input').prop('checked',!$(this).children('.form-check-input').prop('checked'));
            $(this).children('.form-check-input').trigger('change');
        }
    });

    $(document).on('click', '.large-radio', function (b) {
        if(b.target.type !== "radio" && b.target.nodeName !== "LABEL") {
            $(this).children('input')[0].checked = true;
            $($(this).children('input')[0]).trigger('change');
        }
    });

    $(document).keyup(function(e) {
        if (e.key === "Escape") {
            if($('.custom-menu').css('display') !== 'none') {
                menuHide();
            }
       }
    });

    document.addEventListener('mousedown', 
                                function (event) {
                                    if (event.detail > 1) {
                                    event.preventDefault();
                                    }
                                }, 
                                false);    

    if(appSettings.SettingsSidebarCollapsed) {
        navSetTabsWidth();
    }

    $(document).on('shown.bs.tab', '.nav-tabs a', function () {
        navSetTabsWidth();
    });    

    if(window.chrome.webview) {
        window.chrome.webview.addEventListener('message', event => hostHandleMessage(event.data));
    }
});

