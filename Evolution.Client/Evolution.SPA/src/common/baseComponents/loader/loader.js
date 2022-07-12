import React,{ Component,Fragment } from 'react';
class LoaderComponent extends Component{ 
    render(){         
    
        return(
            // <div className='loaders'><div></div><div></div><div></div></div>
            <Fragment>
            { this.props.isShowLoader &&  <div className={ this.props.isShowLoader ? 'loaderOverlay' : 'hide'}>
                    <div className={ this.props.isShowLoader ? 'loaders middelContainer' : 'hide'}><div></div><div></div><div></div></div>
            </div>
            }
            </Fragment>
            
        );
    }
}

export default LoaderComponent;