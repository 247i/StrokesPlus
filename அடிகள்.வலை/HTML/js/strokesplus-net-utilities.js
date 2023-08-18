String.prototype.idname = function() {
    return this.replace(/\W/g,'_');
}

function alertClearFooter(tag) {
    if(tag && tag.length > 0) {
        $('.footer .alert-container').find('.alert[data-tag="' + tag + '"]').remove();
    } else {
        $('.footer .alert-container').html('');
    }
}

function alertShowFooter(tag, className, message, leaveOthers) {
    if(!leaveOthers) {
        alertClearFooter();
    }
    log('alert footer msg: ' + message);
    $('.footer .alert-container').prepend(
            `<div class="float-left mr-1 alert alert-${className} alert-dismissible alert-footer" 
                role="alert" 
                data-tag="${tag}">
                <svg xmlns="http://www.w3.org/2000/svg" type="button" 
                    data-dismiss="alert" 
                    aria-label="Close" 
                    class="close icon icon-tabler icon-tabler-circle-x" 
                    width="25" 
                    height="25" 
                    viewBox="0 0 30 30" 
                    stroke-width="1.2" 
                    stroke="#2c3e50" 
                    fill="none" 
                    stroke-linecap="round" 
                    stroke-linejoin="round">
                    <path stroke="none" d="M0 0h24v24H0z" fill="none"></path>
                    <circle cx="12" cy="12" r="9"></circle>
                    <line x1="16" y1="8" x2="8" y2="16"></line>
                    <line x1="8" y1="8" x2="16" y2="16"></line>
                </svg>
                <div class="container d-flex h-100">
                    <div class="alert-body row justify-content-center align-self-center">
                        ${message}
                    </div>
                </div>
            </div>`
    );
}

function colorNetToHex(color)
{
    var decColor =0x1000000 + color.B + 0x100 * color.G + 0x10000 * color.R ;
    return '#'+decColor.toString(16).substr(1);
}

function colorHexToRGB(hex) {
    var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return result ? {
      r: parseInt(result[1], 16),
      g: parseInt(result[2], 16),
      b: parseInt(result[3], 16)
    } : null;
}

function colorInvert(hex) {
    if (hex.indexOf('#') === 0) {
        hex = hex.slice(1);
    }
    // convert 3-digit hex to 6-digits.
    if (hex.length === 3) {
        hex = hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2];
    }
    if (hex.length !== 6) {
        throw new Error('Invalid HEX color.');
    }
    // invert color components
    var r = (255 - parseInt(hex.slice(0, 2), 16)).toString(16),
        g = (255 - parseInt(hex.slice(2, 4), 16)).toString(16),
        b = (255 - parseInt(hex.slice(4, 6), 16)).toString(16);
    // pad each with zeros and return
    return '#' + stringPadZero(r) + stringPadZero(g) + stringPadZero(b);
}

function hostHandleMessage(data) {
    alert(data);
}

function hostPostMessage(action, message, data) {
    if(window.chrome.webview) {
        window.chrome.webview.postMessage({ action: action, message: message, data: data});
    } else {
        log('hostPostMessage\naction: '+action+'\nmessage: '+message+'\ndata: '+data);
    }
}

function log(msg) {
    //TODO: If enabled
    console.log(msg);
}

function rectViewport() {
    let $w = $(window);
    let rect = {
                  Left: $w.scrollLeft(),
                  Top: $w.scrollTop(),
                  Width: $w.width(),
                  Height:  $w.height()
               };
    return {
               Left: rect.Left,
               Top: rect.Top,
               Right: rect.Left + rect.Width,
               Bottom: rect.Top + rect.Height,
               X: rect.Left,
               Y: rect.Top,
               Width: rect.Width,
               Height: rect.Height
           };
}

function scrollToTop(elementId) {
    log('scrollToTop - elementId: ' + elementId);
    if(elementId) {
        var ele = document.getElementById(elementId);
        ele.scrollTop = 0;
    } else {
        document.body.scrollTop = 0; // For Safari
        document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
    }
}

function settingsUpdateAppSettings()
{
    // TODO: Raise event or something to invoke saving all pending UI changes
}

function settingsApplyClickedCloseConfirm() {

    modalDismissStandardCenter();

    $('#modalStandardCenter').find('.modal-dialog');
    modalStandardCenterTitle = stringEscapeHtml(`||SettingsApplyCancelTitle||`);
    modalStandardShowCallback = "";
    modalStandardCenterBody = stringToHtml(`||SettingsApplyCancelMessage||`);
    modalStandardCenterButtons = "yes|settingsApplyClickedCloseConfirmYes(),no|settingsApplyClickedCloseConfirmNo()";

    $('#modalStandardCenter').modal('show');    
}

function settingsApplyClickedCloseConfirmYes() {
    modalDismissStandardCenter();
    hostPostMessage('CancelConfirm', '', appSettings);
}

function settingsApplyClickedCloseConfirmNo() {
    modalDismissStandardCenter();
    settingsMainFormButtonsEnable();
}

function settingsMainFormButtonsDisable() {
    $('.main-form-buttons .btn').prop('disabled', true);
}

function settingsMainFormButtonsEnable() {
    $('.main-form-buttons .btn').removeAttr('disabled');
}

function settingsResizeElements() {
    if(window.currentSection && window.currentSection.ActionList) {
        log('settingsResizeElements - window.currentSection');
        window.currentSection.ActionList.AdjustHeight();
        if(window.currentSection.ActionList.ActionEditor && window.currentSection.ActionList.ActionEditor.StepsScriptEditor.ScriptEditor.CodeMirror) {
            window.currentSection.ActionList.ActionEditor.StepsScriptEditor.ScriptEditor.Resize();
        }          
    } else {
        log('settingsResizeElements - else');
        navSetTabsWidth();
    }
}

function stringCaseInsensitiveEquals(a, b) {
    return typeof a === 'string' && typeof b === 'string'
        ? a.localeCompare(b, undefined, { sensitivity: 'accent' }) === 0
        : a === b;
}

function stringEscapeHtml(html){
    var text = document.createTextNode(html);
    var p = document.createElement('p');
    p.appendChild(text);
    return p.innerHTML;
}

function stringEscape(s) {
    return ('' + s) /* Forces the conversion to string. */
        .replace(/\\/g, '\\\\') /* This MUST be the 1st replacement. */
        .replace(/\t/g, '\\t') /* These 2 replacements protect whitespaces. */
        .replace(/\n/g, '\\n')
        .replace(/\u00A0/g, '\\u00A0') /* Useful but not absolutely necessary. */
        .replace(/&/g, '\\x26') /* These 5 replacements protect from HTML/XML. */
        .replace(/'/g, '\\x27')
        .replace(/"/g, '\\x22')
        .replace(/</g, '\\x3C')
        .replace(/>/g, '\\x3E')
        ;
}

function stringEscapeProperty(s) {
    return ('' + s)
        .replace(/"/g, "&#34;")
        .replace(/'/g, "&#39;");
}

function stringPadZero(str, len) {
    len = len || 2;
    var zeros = new Array(len).join('0');
    return (zeros + str).slice(-len);
}

function stringTextHeight(font) {

    var text = $('<span>Hg</span>').css('font', font );
    var block = $('<div style="display: inline-block; width: 1px; height: 0px;"></div>');
  
    var div = $('<div></div>');
    div.append(text, block);
  
    var body = $('body');
    body.append(div);
  
    try {
  
      var result = {};
  
      block.css({ verticalAlign: 'baseline' });
      result.ascent = block.offset().top - text.offset().top;
  
      block.css({ verticalAlign: 'bottom' });
      result.height = block.offset().top - text.offset().top;
  
      result.descent = result.height - result.ascent;
  
    } finally {
      div.remove();
    }
  
    return result;
}

function stringToHtml(s){
    return ('' + s) /* Forces the conversion to string. */
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/\t/g, '&nbsp;&nbsp;&nbsp;&nbsp;') 
        .replace(/\n/g, '<br />')        
        ;
}

