// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { AppBar, Box, Button, Link, Menu, MenuItem, Typography } from '@mui/material';
import { Stack } from '@mui/system';
import { useNavigate } from 'react-router-dom';

export const Header = () => {
    const navigate = useNavigate();

    return (
        <div>
            <Box sx={{ flexGrow: 1 }}>
                <AppBar position="static">
                    <Stack spacing={2} direction="row">
                        <Button variant="contained" onClick={() => navigate('/')} >Home</Button>
                        <Button variant="contained" onClick={() => navigate('/applications')}> Applications</Button>
                        <Button variant="contained" onClick={() => navigate('/organization')}>Organization settings</Button>
                    </Stack>
                </AppBar>
            </Box>
        </div >
    );
};
