package sk.spacecode.smartbottle

import android.bluetooth.BluetoothAdapter
import android.bluetooth.BluetoothDevice
import android.bluetooth.BluetoothSocket
import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import android.content.IntentFilter
import android.os.AsyncTask
import android.os.Bundle
import android.support.v7.app.AppCompatActivity
import android.util.Log
import android.view.View
import kotlinx.android.synthetic.main.activity_connect_to_bottle.*
import java.io.IOException
import java.util.*

class ConnectToBottleActivity : AppCompatActivity() {

    companion object Socket{
        var bluetoothSocket: BluetoothSocket? = null
    }

    private val bluetoothRequestCode = 1
    private val myUUID = UUID.fromString("00001101-0000-1000-8000-00805F9B34FB")
    private var device: BluetoothDevice? = null
    private var isDeviceConnected = false
    private var bluetoothAdapter: BluetoothAdapter? = null
    private val logConstantClass = "ConnectToBottleActivity"

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_connect_to_bottle)

        // Checking if device has Bluetooth

        bluetoothAdapter = BluetoothAdapter.getDefaultAdapter()

        if (bluetoothAdapter == null) {
            finish()
        }

        // Asking to enable Bluetooth, if it is off

        if (!bluetoothAdapter!!.isEnabled) {
            val enableBtIntent = Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE)
            startActivityForResult(enableBtIntent, bluetoothRequestCode)
        }
        ConnectToArduino().execute()
    }

    inner class ConnectToArduino : AsyncTask<Void, Void, Void>() {
        private var connectSuccess = true

        override fun doInBackground(vararg devices: Void): Void? {

            try {
                device = bluetoothAdapter?.getRemoteDevice(intent.getStringExtra("qr_result").trim())
                Log.i(logConstantClass, device?.address)

                if (device?.bondState == BluetoothDevice.BOND_BONDED) {
                    if (bluetoothSocket == null || !isDeviceConnected) {
                        bluetoothSocket = device?.createInsecureRfcommSocketToServiceRecord(myUUID)
                        bluetoothSocket!!.connect()
                    }
                }

            } catch (e: IOException) {
                connectSuccess = false
            }
            return null
        }

        override fun onPostExecute(result: Void?) {
            super.onPostExecute(result)

            if (!connectSuccess) {
                Log.i(logConstantClass, "Connection Failed")
                ConnectToArduino().execute()
            } else {
                Log.i(logConstantClass, "Connection Success")
                isDeviceConnected = true
                connect_to_bottle_loader.visibility = View.GONE
                connect_to_bottle_label.visibility = View.GONE
                val intent = Intent(this@ConnectToBottleActivity, RegisterBottleActivity()::class.java)
                intent.putExtra("device_mac", device?.address)
                startActivity(intent)
            }
        }
    }
}
