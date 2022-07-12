import React, { Component } from 'react';
import { EditorState, ContentState } from 'draft-js';
import { Editor } from 'react-draft-wysiwyg';
import '../../../../node_modules/react-draft-wysiwyg/dist/react-draft-wysiwyg.css';

class Editer extends Component {
    constructor(props) {
      super(props);
      this.state = {
        editorState: EditorState.createWithContent(
            ContentState.createFromText(this.props.data)
        ),
      };
    }
    componentWillReceiveProps(){
        this.setState({ editorState: EditorState.createWithContent(
            ContentState.createFromText(this.props.data)
        ) });
    }
  
    onEditorStateChange = (editorState) => {
      this.setState({
        editorState,
      });
    };

  render() {      
    return <Editor editorState={this.state.editorState} 
    onEditorStateChange={this.onEditorStateChange} />;
  }
}

export default Editer;