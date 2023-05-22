function SendData(datos)
    ipAddress = char(java.net.InetAddress.getLocalHost.getHostAddress);
    tcpipClient = tcpclient(ipAddress,55001);
    set(tcpipClient,'Timeout',3);
    %open(tcpipClient);
    Data=datos;
    write(tcpipClient,Data);
    %close(tcpipClient);
end