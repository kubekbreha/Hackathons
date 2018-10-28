package sk.spacecode.smartbottle

import android.Manifest
import android.content.Intent
import android.content.pm.PackageManager
import android.os.Build
import android.os.Bundle
import android.support.v7.app.AppCompatActivity
import android.util.Log
import android.widget.Toast

import com.budiyev.android.codescanner.CodeScanner

import androidx.annotation.NonNull

class CodeScannerActivity : AppCompatActivity() {
    private var mCodeScanner: CodeScanner? = null
    private var mPermissionGranted: Boolean = false


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_code_scanner)

        mCodeScanner = CodeScanner(this, findViewById(R.id.scanner))
        mCodeScanner!!.setDecodeCallback { result ->
            val intent = Intent(this, ConnectToBottleActivity::class.java)
            intent.putExtra("qr_result", result.toString().trim())
            startActivity(intent)
        }

        mCodeScanner!!.setErrorCallback {
            runOnUiThread {
                Toast.makeText(this, "SCANNER ERROR", Toast.LENGTH_LONG).show()
            }
        }

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            if (checkSelfPermission(Manifest.permission.CAMERA) !== PackageManager.PERMISSION_GRANTED) {
                mPermissionGranted = false
                requestPermissions(arrayOf(Manifest.permission.CAMERA), RC_PERMISSION)
            } else {
                mPermissionGranted = true
            }
        } else {
            mPermissionGranted = true
        }
    }


    override fun onRequestPermissionsResult(
        requestCode: Int, @NonNull permissions: Array<String>,
        @NonNull grantResults: IntArray
    ) {
        if (requestCode == RC_PERMISSION) {
            if (grantResults.isNotEmpty() && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                mPermissionGranted = true
                mCodeScanner!!.startPreview()
            } else {
                mPermissionGranted = false
            }
        }
    }

    override fun onResume() {
        super.onResume()
        if (mPermissionGranted) {
            mCodeScanner!!.startPreview()
        }
    }

    override fun onPause() {
        mCodeScanner!!.releaseResources()
        super.onPause()
    }

    companion object {
        private val RC_PERMISSION = 10
    }
}
