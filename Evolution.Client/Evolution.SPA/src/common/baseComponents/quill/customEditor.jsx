import React from 'react';
import ReactQuill from 'react-quill';
import 'react-quill/dist/quill.snow.css';
import PropTypes from 'prop-types';
// ToDo - Rich Editor Css
import './quill.css';
import sanitize from 'sanitize-html';

class Editor extends React.Component {
    constructor (props) {
      super(props);
      this.theme= 'snow' ;
    }

    //Added for Def1187 (#4 Issue Failed on 30-09-2020)
    componentDidMount(){
      const input = document.querySelector(
        "input[data-link]"
        );
        input.dataset.link = "";
    } //Added for Def1187 (#4 Issue Failed on 30-09-2020)
    
    shouldComponentUpdate(nextProps, nextState){
      if(this.props.className === nextProps.className && this.props.editorHtml === nextProps.editorHtml) //IGO QC D928 Fix
          return false;
      return true;
  }
    render () {
      return (
          <ReactQuill 
            className={this.props.className ? this.props.className : ''}//sanity def 182 fix
            theme={this.theme}
            onChange={this.props.onChange}
            value={this.props.editorHtml} // Security Issue "Stored Cross Site Scripting" fix (https://github.com/apostrophecms/sanitize-html)
            modules={Editor.modules}
            formats={Editor.formats}
            bounds={'.app'}
            placeholder={this.props.placeholder}
            readOnly={this.props.readOnly ? this.props.readOnly : false}
           />
       );
    }
  }
   
  /* 
   * Quill modules to attach to editor
   * See https://quilljs.com/docs/modules/ for complete options
   */
  Editor.modules = {
    toolbar: [
      [ { 'header': [ 1, 2, false ] } ], 
      [  'italic', 'bold','underline', 'strike', 'blockquote' ],
      [ { 'list': 'ordered' }, { 'list': 'bullet' }, 
       { 'indent': '-1' }, { 'indent': '+1' } ],
      [ 'link', ],
      [ 'clean' ]
    ],
    clipboard: {
      matchVisual: false,
    } //Uncommented for D1187 #1 Issue Failed by on 24-10-2020
  };
  /* 
   * Quill editor formats
   * See https://quilljs.com/docs/formats/
   */
  Editor.formats = [
    'header', 'font', 'size',
    'bold', 'italic', 'underline', 'strike', 'blockquote',
    'list', 'bullet', 'indent',
    'link'
  ];
  
  /* 
   * PropType validation
   */
  Editor.propTypes = {
    placeholder: PropTypes.string,
  };

  export default Editor;