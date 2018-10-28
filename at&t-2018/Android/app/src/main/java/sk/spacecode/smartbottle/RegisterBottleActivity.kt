package sk.spacecode.smartbottle

import android.content.Intent
import android.os.Bundle
import android.support.v7.app.AppCompatActivity
import android.widget.Toast
import com.google.firebase.database.*
import kotlinx.android.synthetic.main.activity_register_bottle.*
import sk.spacecode.smartbottle.dataClasses.Bottle


class RegisterBottleActivity : AppCompatActivity() {

    private var mDatabase: DatabaseReference? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_register_bottle)
        activity_register_bottle_bottleId.maxLines = 1


        val bottleId = intent.getStringExtra("device_mac")

        mDatabase = FirebaseDatabase.getInstance().reference
        activity_register_bottle_bottleId.text = bottleId

        activity_register_bottle_firebase.setOnClickListener {

            val password = activity_personal_details_name_input.text

            if (password.isNotEmpty()) {
                val bottle = Bottle(bottleId, password.toString().trim())

                //todo: add default value to
                mDatabase!!.child(bottleId).child("drinkedWather").setValue(120)

                mDatabase!!.child(bottleId).child("login").setValue(bottle).addOnCompleteListener { task ->
                    when {
                        task.isSuccessful -> {

                            val intent = Intent(this, PersonalDetailsActivity::class.java)
                            intent.putExtra("device_mac", bottleId)
                            startActivity(intent)
                        }
                        else -> {
                            Toast.makeText(this, "Push unsucessfull.", Toast.LENGTH_SHORT).show()
                        }
                    }
                }


            } else {
                Toast.makeText(this, "Please use longer password.", Toast.LENGTH_SHORT).show()
            }
        }
    }


    private fun addDrinkedWather() {

    }

}
