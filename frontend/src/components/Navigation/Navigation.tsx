import {Menu} from "antd";
import {useState} from "react";
import {useNavigate} from "react-router";

const items = [
    {
        key: '1',
        label: 'Sessions',
        route: '/sessions'
    },
    {
        key: '2',
        label: 'Fuzzing Presets',
        route: '/presets'
    }
];

const Navigation = () => {
    const
        navigate = useNavigate(),
        [selectedTab, setSelectedTab] = useState('1');

    const onClick = (a: {key: string}) => {
        const item = items.find(i => i.key == a.key)!;

        setSelectedTab(item.key)
        navigate(item.route);
    };

    return (
        <Menu
            theme="dark"
            mode="horizontal"
            items={items}
            selectedKeys={[selectedTab]}
            style={{
                flex: 1,
                minWidth: 0,
            }}
            onClick={onClick}
        />
    );
};

export default Navigation;