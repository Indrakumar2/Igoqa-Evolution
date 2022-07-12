import React, { Component } from 'react';
import { Helmet } from 'react-helmet';

class Title extends Component { 
    
    constructor(props)
    {
        super(props);
        document.title = this.props.title;
    }

    render() {
        return (
            <Helmet
                title={this.props.title}>
            </Helmet>
        );
    }
}

export default Title;
