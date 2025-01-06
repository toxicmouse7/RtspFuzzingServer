import {Layout, theme} from "antd";
import {Outlet} from "react-router";
import {Navigation} from "./components";

const {Header, Content} = Layout;

const AppLayout = () => {
    const {
        token: { colorBgContainer, borderRadiusLG },
    } = theme.useToken();

    return (
        <Layout style={{ height: "100vh" }}>
            <Header>
                <Navigation/>
            </Header>
            <Content style={{padding: "48px"}}>
                <div
                    style={{
                        background: colorBgContainer,
                        minHeight: 280,
                        padding: 24,
                        borderRadius: borderRadiusLG,
                    }}
                >
                    <Outlet/>
                </div>
            </Content>
        </Layout>
    );
};

export default AppLayout;