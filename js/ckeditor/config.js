/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here.
    // For complete reference see:
    // http://docs.ckeditor.com/#!/api/CKEDITOR.config

    // The toolbar groups arrangement, optimized for two toolbar rows.
    config.toolbarGroups = [
		{ name: 'clipboard', groups: ['clipboard', 'undo'] },
		{ name: 'editing', groups: ['find', 'selection', 'spellchecker'] },
		{ name: 'links' },
	    { name: 'insert' }, '/',
	    { name: 'forms' },
	    { name: 'tools' },
	    { name: 'document', groups: ['mode', 'document', 'doctools'] },
	    { name: 'others' },
       { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi'] },
        { name: 'Format' },

	    { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },

		{ name: 'styles' },
        { name: 'colors' },
        { name: 'others' }
    ];
    //config.filebrowserImageBrowseUrl = '/Secure/DocForms';
    //config.filebrowserImageUploadUrl = '/Secure/DocForms';

    // Remove some buttons provided by the standard plugins, which are
    // not needed in the Standard(s) toolbar.
    //config.removeButtons = 'Underline,Subscript,Superscript';
    config.extraPlugins = 'font,colorbutton';
    // Set the most common block elements.
    config.format_tags = 'p;h1;h2;h3;pre';
    config.removeDialogTabs = 'image:advanced';
    //if (navigator.userAgent.search("Firefox") >= 0) {
    config.height = '170';
   // config.width = '600';
    //}
    //"CKEDITOR.config.htmlEncodeOutput = true;"
    //config.forcePasteAsPlainText = true;
    // Simplify the dialog windows.
    //config.removeDialogTabs = 'image:advanced;link:advanced';
};
