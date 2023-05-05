using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using System;
using Unity.Services.Authentication;
using System.Linq;

public class RelayManager : Singleton<RelayManager>
{
    [SerializeField] private string environment = "production";

    [SerializeField] private int maxConnections = 4;

    public UnityTransport Transport => NetworkManager.Singleton.GetComponent<UnityTransport>();
    public bool IsRelayEnabled => Transport != null && Transport.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;

    public async Task<RelayHostData> SetupRelay() {
        InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment);

        await UnityServices.InitializeAsync(options);

        if (!AuthenticationService.Instance.IsSignedIn) {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        Allocation allocation;
        string createJoinCode;
        try {
            allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        }
        catch (Exception e) {
            Debug.LogError($"Relay create allocation request failed {e.Message}");
            throw;
        }

        Debug.Log($"server: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
        Debug.Log($"server: {allocation.AllocationId}");

        try {
            createJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        }
        catch {
            Debug.LogError("Relay create join code request failed");
            throw;
        }

        Debug.Log($"server: {createJoinCode}");

        var dtlsEndpoint = allocation.ServerEndpoints.First(e => e.ConnectionType == "dtls");
        RelayHostData relayHostData = new RelayHostData {
            key = allocation.Key,
            port = (ushort)dtlsEndpoint.Port,
            allocationId = allocation.AllocationId,
            allocationIdBytes = allocation.AllocationIdBytes,
            ipv4address = dtlsEndpoint.Host,
            connectionData = allocation.ConnectionData,
            joinCode = createJoinCode,
        };

        Transport.SetHostRelayData(relayHostData.ipv4address, relayHostData.port, relayHostData.allocationIdBytes, relayHostData.key, relayHostData.connectionData, true);

        return relayHostData;
    }

    public async Task<RelayJoinData> JoinRelay(string joinCode) {
        InitializationOptions options = new InitializationOptions().SetEnvironmentName(environment);

        await UnityServices.InitializeAsync(options);

        if (!AuthenticationService.Instance.IsSignedIn) {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        JoinAllocation allocation;
        try {
            allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        }
        catch {
            Debug.LogError("Relay create join code request failed");
            throw;
        }

        Debug.Log($"client: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
        Debug.Log($"host: {allocation.HostConnectionData[0]} {allocation.HostConnectionData[1]}");
        Debug.Log($"client: {allocation.AllocationId}");

        var dtlsEndpoint = allocation.ServerEndpoints.First(e => e.ConnectionType == "dtls");
        RelayJoinData relayJoinData = new RelayJoinData {
            ipv4address = dtlsEndpoint.Host,
            port = (ushort)dtlsEndpoint.Port,
            allocationIdBytes = allocation.AllocationIdBytes,
            connectionData = allocation.ConnectionData,
            hostConnectionData = allocation.HostConnectionData,
            key = allocation.Key,
            joinCode = joinCode,
        };

        Transport.SetClientRelayData(relayJoinData.ipv4address, relayJoinData.port, relayJoinData.allocationIdBytes, relayJoinData.key, relayJoinData.connectionData, relayJoinData.hostConnectionData, true);

        return relayJoinData;
    }
}

public class RelayHostData
{
    public string ipv4address; public ushort port; public byte[] allocationIdBytes; public byte[] connectionData; public byte[] key; public string joinCode;
    public Guid allocationId;
}

public class RelayJoinData
{
    public string ipv4address; public ushort port; public byte[] allocationIdBytes; public byte[] connectionData; public byte[] hostConnectionData; public byte[] key;
    public string joinCode;
}