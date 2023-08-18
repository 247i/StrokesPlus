class ScriptEditor {
    constructor(actionEditorId, textArea, baseAction) {
        $(`#${textArea}`).text(baseAction.Script);
        this.Id = `codemirror_${baseAction.Description.idname()}_${Date.now()}`;
        this.ActionEditorId = actionEditorId;
        this.Action = baseAction;
        this.CodeMirror = CodeMirror.fromTextArea(document.getElementById(textArea), {
                                                     lineNumbers: true,
                                                     autoRefresh: true,
                                                     mode: "javascript",
                                                     matchBrackets: true,
                                                     extraKeys: {"Ctrl-Space": "autocomplete"},
                                                     viewportMargin: Infinity
                                                  });
        this.CodeMirror.setSize(100, 100);
        this.Resize();
    }

    HideEditor() {
        //$(this.CodeMirror.getWrapperElement()).css('visibility', 'hidden');
        $(this.CodeMirror.getWrapperElement()).hide();
    }

    Record() {
        log('Record()');
    }

    Resize() {
        let actionEditorElement = $(`#${this.ActionEditorId}`);
        let toggle = $(actionEditorElement).find('.ToggleScriptEditorParent');

        let left = parseInt($(toggle).position().left) + parseInt($('.sidebar-sticky').width());
        let width = rectViewport().Right - left;
        let top = parseInt($(toggle).position().top) + parseInt($(toggle).height());
        let height = $('.footer').position().top - top;

        this.CodeMirror.setSize(width-15, height);
    }

    Save() {
        this.Action.Script = this.CodeMirror.getDoc().getValue();
    }

    ShowEditor() {
        //$(this.CodeMirror.getWrapperElement()).css('visibility', 'visible');
        $(this.CodeMirror.getWrapperElement()).show();
    }

}