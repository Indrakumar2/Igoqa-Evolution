import React, { Component } from 'react';
import MaterializeComponent from 'materialize-css';
import IntertekToaster from '../intertekToaster';
class HyperLink extends Component {     

    render() {
        return (
            <a onClick={()=>IntertekToaster('Under Construction','warningToast underConstruction')} className="link">{this.props.value}</a>
            // {this.props.data[this.props.dataToRender]}
        );
    }
}

export default HyperLink;