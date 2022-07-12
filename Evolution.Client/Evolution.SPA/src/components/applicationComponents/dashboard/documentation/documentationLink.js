import React, { Component } from 'react';

class DocumentationLink extends Component {   
    render() { 
        return (
            <a  className="link" href={this.props.data.description} target="_blank">{this.props.value} </a>     
        );
    }
}

export default DocumentationLink;